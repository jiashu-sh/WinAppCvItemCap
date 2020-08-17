# OpenCvSharp辅助DataMatrix二维码扫描

由于硬件二维码扫描头只能输出二维码内容，而在定位需求中，我还希望能知道扫描到的具体时间（解码也需要时间，但是硬件体现不出来），以及位置信息，所以尝试自己用摄像头做解码。
从某宝上买了一个120度广角免驱摄像头，因为这货就是个小PCB板，好处是可以在购买时选择各种参数，但是不易站立，于是翻出我的电子垃圾组合了一下，如下图：
![github](
https://upload-images.jianshu.io/upload_images/24193214-9f75997cf83bd99a.PNG?imageMogr2/auto-orient/strip|imageView2/2/w/546/format/webp
"github")

先试了下摄像头帧率，通过日志得出，大概确实是30帧左右，也就是每帧时间30ms+。
首先试了下直接将每帧传入解码，使用的是DataMatrix.net组件，代码：
private static DataMatrix.net.DmtxImageDecoder dmtxImageDecoder = new DataMatrix.net.DmtxImageDecoder();
List<string> lstDataMatrixCode = dmtxImageDecoder.DecodeImage(BitmapConverter.ToBitmap(m.DmMat));
实测640x480的图片解码需要4~6s （i75600 2.4GHz 笔记本电脑）

所以基本思路就是将每帧的图片进行裁剪，只提取需要的区域，能大大减少运算量，同时，需要寻找二维码的区域，也可以定位二维码的所在位置。代码如下：
// 转换颜色空间，可以转化为yuv，下面是转化为灰度图
                Cv2.CvtColor(matDataMatrixSrc, matBinary, ColorConversionCodes.BGR2GRAY);
                //这里得到的二值图matBinary作为最后检测二维码的源
                //二值化:参数3为阈值，4为大于阈值的像素灰度值，5为二值化类型                
                double dThreshould = 70;
                double dMaxVal = 235;
                Cv2.Threshold(matBinary, matBinary, dThreshould, dMaxVal, ThresholdTypes.Binary);
经过边缘提取的效果如下：
![github](
https://upload-images.jianshu.io/upload_images/24193214-64a8522c3a951675.png?imageMogr2/auto-orient/strip|imageView2/2/w/640/format/webp
"github")
 
如上图的效果不太理想，于是我尝试了做了高级形态变化之后再提取轮廓，效果如下：
 
效果还是不行，貌似还不如前一个版本。优化思路是考虑到模拟人类的视觉方式，所以先进行高强度的模糊化，然后进行二值化，再提取轮廓。效果如下：
![github](
https://upload-images.jianshu.io/upload_images/24193214-bf302a9178e28a0c.png?imageMogr2/auto-orient/strip|imageView2/2/w/640/format/webp
"github")
 
对提取二维码的mat做了剧烈的模糊之后的效果好多了，基本上可以识别出大概的二维码所在范围了。下方的阴影区域属于干扰数据，后文再考虑如何优化去除。
接下来调整参数之后，绘制内接圆，并在内接圆的基础上绘制正方形区域，代码和效果如下：

![github](
https://upload-images.jianshu.io/upload_images/24193214-5795993d5b34d62d.png?imageMogr2/auto-orient/strip|imageView2/2/w/640/format/webp
"github")

OpenCvSharp.Point2f center;//圆心坐标
                float radius;//园半径
//寻找最小外接圆，输出中心点和半径
                    Cv2.MinEnclosingCircle(points[i], out center, out radius);
//绘制检测到的区域的正方形
                    Cv2.Rectangle(matDataMatrixSrc, pt1, pt2, Scalar.Green, 1, LineTypes.Link8);
 
然后继续优化一下具体参数，去掉内接圆和边缘绘制，效果如下：

![github](
https://upload-images.jianshu.io/upload_images/24193214-56ea9e6dd9b0a51e.png?imageMogr2/auto-orient/strip|imageView2/2/w/640/format/webp
"github")
 
下面的绿色大方块是检测到的阴影的干扰，考虑到阴影原图基本上是没有色彩变化的，和二维码的剧烈变化有很大差别，所以可以采用对原图去取阈值之后二值化，再用灰色色块对目标区域进行xor运算，然后统计xor之后，1的数量在总像素的占比，低于5%的话，即认为这是一张空白图，关键代码和效果如下：
Scalar s = new Scalar(235, 235, 235);//定义三通道颜色
            Mat n = new Mat(m.Rows, m.Cols, MatType.CV_8UC1, s); //这里要用CV_8UC1，如果是载入图片用CV_8UC3，单色图片和rgb的区别
            Cv2.BitwiseXor(m, n, r); //xor运算一下，把不是235的颜色通过xor置为1

界面如下：
![github](
https://upload-images.jianshu.io/upload_images/24193214-c238b15856e4bc65.png?imageMogr2/auto-orient/strip|imageView2/2/w/640/format/webp
"github") 

上图底部的灰色方框即为最后被排除的干扰。
接下来是对性能的优化，首先减少捕捉的帧运算，目标是每秒检测10次，故每3帧进行一次运算即可。由于dmtxImageDecoder.DecodeImage运算时间较长，故不能直接在主进程中而通过异步线程去执行，基本上可以得到一个较为可接受的结果。
最终效果如下：
 
最终效果就是把检测到的二维码显示在界面上。

![github](
https://upload-images.jianshu.io/upload_images/24193214-66dfe7d60103a853.png?imageMogr2/auto-orient/strip|imageView2/2/w/640/format/webp
"github")


