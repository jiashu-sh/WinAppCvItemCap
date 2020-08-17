using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using WinAppCvItemCap01.Common;

namespace WinAppCvItemCap01
{
    public partial class FormCvItemCap : Form
    {
        private Image imgshow;

        private static bool bDetectOn = false; //是否开始侦测
        private static bool bShowMoving = false; //是否看实时画面
        private static bool bShowPic = false; //输出到图片Pb

        private static bool bStartFindDmCode = false; //是否进行DataMatrix解码
        private static bool bDetectLines = false; //是否进行线段检测

        private static List<EntityDataMatrixMat> ListEntityDataMatrixMats;
        private static List<EntityDataMatrixMat> ListEffectiveEntityDataMatrixMats;


        /// <summary>
        /// 用于获取或存储摄像头设备变量
        /// </summary>
        internal static VideoCapture VideoCapture_OS;

        private static DataMatrix.net.DmtxImageDecoder dmtxImageDecoder = new DataMatrix.net.DmtxImageDecoder();


        public delegate void InvokeUpdateTextbox(string strtb);
        public void SetText(string strtb)
        {
            tbMessage.Text = strtb;
        }


        public delegate void InvokeDecodeDmCode();
        public void DecodeDmCode()
        {

            tbDmCodes.Text = "";

            string sInfo = "";

            sInfo = "";

            DecodeMethod();

            if (ListEffectiveEntityDataMatrixMats == null)
                return;
            if (ListEffectiveEntityDataMatrixMats.Count == 0)
                return;

            

            foreach (EntityDataMatrixMat me in ListEffectiveEntityDataMatrixMats)
            {
                sInfo += "[ " + me.DmCode + " : " + me.RunTimeSpanMs.ToString() + " ] " + "(" + me.CenterPt.X +"," + me.CenterPt.Y + ") ";
            }

            tbDmCodes.Text = sInfo;
        }

        public static async void DecodeMethod()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} start");
            await Task.Run(new Action(LongTaskDecodeToEffectiveList));
            Console.WriteLine("method");
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} end");
        }

        /// <summary>
        /// 较为耗时的解码过程
        /// </summary>
        public static void LongTaskDecodeToEffectiveList()
        {
            if (ListEntityDataMatrixMats == null)
                return;
            if (ListEntityDataMatrixMats.Count == 0)
                return;


            for (int i = 0; i < ListEntityDataMatrixMats.Count; i++)
            {

                EntityDataMatrixMat m = ListEntityDataMatrixMats[i];
                if (m.IsDecoded)
                    continue;

                bool bIsSimilar = false;
                //在有效的二维码类里找看看是否有相似图片
                if (ListEffectiveEntityDataMatrixMats != null)
                {
                    for (int j = 0; j < ListEffectiveEntityDataMatrixMats.Count; j++)
                    {
                        if (CompareMatSimilar(ListEffectiveEntityDataMatrixMats[j].DmMat, m.DmMat))
                        {
                            ListEntityDataMatrixMats[i].IsDecoded = true;
                            ListEntityDataMatrixMats[i].RunTimeSpanMs = 1;
                            ListEntityDataMatrixMats[i].DmCode = ListEffectiveEntityDataMatrixMats[j].DmCode;
                            ListEntityDataMatrixMats[i].ParentTimeStampId = ListEffectiveEntityDataMatrixMats[j].TimeStampId;
                            bIsSimilar = true;
                            break;
                        }
                    }
                    if (bIsSimilar)
                        continue;
                }

                //return;

                //检测程序运行时间
                DateTime beforeDT2 = System.DateTime.Now;
                bool bIsFindDmCode = false;
                //时间比较长的二维码检查，实测消耗4~6s(640x480),现通过算法进行寻找二维码区域剪裁后，单个识别速度大大提高
                List<string> lstDataMatrixCode = dmtxImageDecoder.DecodeImage(BitmapConverter.ToBitmap(m.DmMat));
                DateTime afterDT2 = System.DateTime.Now;
                TimeSpan ts2 = afterDT2.Subtract(beforeDT2);
                if (lstDataMatrixCode.Count > 0)
                {
                    bIsFindDmCode = true;
                    foreach (string s in lstDataMatrixCode)
                    {
                        ListEntityDataMatrixMats[i].IsDecoded = true;
                        ListEntityDataMatrixMats[i].DmCode = s;
                        ListEntityDataMatrixMats[i].RunTimeSpanMs = ts2.TotalMilliseconds;

                        m.IsDecoded = true;
                        m.DmCode = s;
                        m.RunTimeSpanMs = ts2.TotalMilliseconds;
                        ListEffectiveEntityDataMatrixMats.Add(m);

                        //tbDmCodes.Text = "[ " + m.DmCode + " : " + m.RunTimeSpanMs.ToString() + " ] " + "(" + m.CenterPt.X + "," + m.CenterPt.Y + ") "; ;

                        if (bIsFindDmCode)
                            break;
                    }

                }
                else
                {
                    ListEntityDataMatrixMats[i].IsDecoded = true;
                    ListEntityDataMatrixMats[i].RunTimeSpanMs = ts2.TotalMilliseconds;
                }

            }
        }

        /// <summary>
        /// 比较Mat是否相似
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static bool CompareMatSimilar(Mat m, Mat n)
        {
            bool bCompareMatSimilar = false;

            if (m == null)
                return false;
            if (n == null)
                return false;


            int Xm = 0;
            int Ym = 0;
            int Xn = 0;
            int Yn = 0;

            int iWidth = m.Width; //暂默认m宽度为待截取区域宽度
            if (m.Width > n.Width)
            {
                //m宽了
                iWidth = n.Width;
                Xm = (m.Width - n.Width) / 2; //m宽了，偏移m
            }
            else
                Xn = (n.Width - m.Width) / 2;

            int iHeight = m.Height; //暂默认n高度为待截取区域高度
            if (m.Height > n.Height)
            {
                //m高了
                iHeight = n.Height;
                Ym = (m.Height - n.Height) / 2; //n高了,偏移m
            }
            else
                Yn = (n.Height - m.Height) / 2;

            //建立m区域
            Rect rectCutoutM = new Rect(Xm, Ym, iWidth, iHeight);
            Mat matCompareM = m[rectCutoutM]; //裁剪
            //建立n区域
            Rect rectCutoutN = new Rect(Xn, Yn, iWidth, iHeight);
            Mat matCompareN = n[rectCutoutN]; //裁剪

            Scalar s = new Scalar(0, 0, 0);//定义3通道颜色
            Mat r = new Mat(m.Rows, m.Cols, MatType.CV_8UC1, s);

            //DateTime beforeDT = System.DateTime.Now;

            Cv2.BitwiseXor(matCompareM, matCompareN, r); //xor运算一下，把不同的颜色通过xor置为1

            int iNoZero = CountMatVal(r);
            int iPoints = r.Height * r.Width;
            const int iPercentage = 10;

            if (((iNoZero * 100) / iPoints) <= iPercentage)
                bCompareMatSimilar = true;

            return bCompareMatSimilar;

            //DateTime afterDT = System.DateTime.Now;
            //TimeSpan ts = afterDT.Subtract(beforeDT);

            //tbResult.Text = "iNoZero = " + iNoZero.ToString() + " , " + "iPoints = " + iPoints.ToString() + " : " + ((iNoZero * 100) / iPoints).ToString() + "%";

            //if (bIsBlank)
            //    tbResult.Text += " Blank img.";

            //tbResult.Text += "[" + ts.TotalMilliseconds + "]";
        }

        public FormCvItemCap()
        {
            InitializeComponent();
        }

        private void FormCvItemCap_Load(object sender, EventArgs e)
        {
            bDetectOn = cbDetectOn.Checked;
            bShowMoving = cbShowMoving.Checked;

            bShowPic = cbShowPic.Checked;

            bStartFindDmCode = cbStrartDecode.Checked;

            bDetectLines = cbDetectLines.Checked;

            ListEntityDataMatrixMats = new List<EntityDataMatrixMat>();
            ListEffectiveEntityDataMatrixMats = new List<EntityDataMatrixMat>();
        }

        private void cbDetectOn_CheckedChanged(object sender, EventArgs e)
        {
            bDetectOn = cbDetectOn.Checked;
        }

        private void cbShowMoving_CheckedChanged(object sender, EventArgs e)
        {
            bShowMoving = cbShowMoving.Checked;
        }

        private void cbShowPic_CheckedChanged(object sender, EventArgs e)
        {
            bShowPic = cbShowPic.Checked;
        }

        private void cbStrartDecode_CheckedChanged(object sender, EventArgs e)
        {
            bStartFindDmCode = cbStrartDecode.Checked;
        }

        private void cbDetectLines_CheckedChanged(object sender, EventArgs e)
        {
            bDetectLines = cbDetectLines.Checked;
        }

        private void cbCheckUnCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            bool bTemp = cbCheckUnCheckAll.Checked;
            cbDetectOn.Checked = bTemp;
            cbShowMoving.Checked = bTemp;
            cbShowPic.Checked = bTemp;
            cbStrartDecode.Checked = bTemp;
            cbDetectLines.Checked = bTemp;
        }

        private void pbCameraView_Paint(object sender, PaintEventArgs e)
        {
            if (bShowMoving)
            {
                Invalidate();

                pbCameraView.Invalidate();

                e.Graphics.DrawImage(imgshow, 0, 0);
            }
        }

        /// <summary>
        /// 初始化摄像头并设置摄像头宽高参数
        /// </summary>
        /// <returns>摄像头调用对象</returns>
        private static VideoCapture InitVideoCapture()
        {
            if (VideoCapture_OS == null)
            {
                VideoCapture_OS = OpenCvSharp.VideoCapture.FromCamera(CaptureDevice.Any);
                VideoCapture_OS.Set(CaptureProperty.FrameWidth, 640);
                VideoCapture_OS.Set(CaptureProperty.FrameHeight, 480);
            }

            return VideoCapture_OS;
        }


        private void btnStartDetection_Click(object sender, EventArgs e)
        {
            Thread threadA = new Thread(RunCapture);

            threadA.Start();
        }

        void RunCapture()
        {
            string sInfo = "近点距离: ";

            //原始帧Mat
            Mat matSrc = new Mat();

            FrameSource frame = Cv2.CreateFrameSource_Camera(0);


            frame.NextFrame(matSrc);
            //Console.WriteLine("w: " + matSrc.Width.ToString() + " x h: " + matSrc.Height.ToString());
            sInfo = "w: " + matSrc.Width.ToString() + " x h: " + matSrc.Height.ToString();
            sInfo = ""; //暂时置为空

            int iVideoWidth = matSrc.Width;
            int iVideoHeight = matSrc.Height;

            Mat matHsv = new Mat(); //转换为hsv
            Mat matMask = new Mat(); //蒙板
            Mat matShow = new Mat();

            Mat matGraymatGray = new Mat(); //v1 简单二值化-先转成灰度图
            Mat matGaussianBlur = new Mat(); //高斯模糊
            Mat matCanny = new Mat();//边缘检测
            Mat matHoughLines = new Mat();//霍夫变换
            
            Mat matDst = new Mat();
            int iCount = 0;
            long CurrentTimeStamp = 0;
            long lDeltaTime = 0;
            Mat matDataMatrixSrc = new Mat(); //进行
            Mat matGaussianBlurDm = new Mat(); //高斯模糊
            Mat matBinary = new Mat(); //二值化 和 之前的灰度用

            Bitmap bitmap;
            while (bDetectOn)//(f1)
            {                
                
                try
                {
                    frame.NextFrame(matSrc);
                    matDataMatrixSrc = matSrc.Clone(); //复制到DataMatrix的Mat
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    continue;
                }

                try
                {
                    iVideoWidth = matSrc.Width;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    continue;
                }

                //Bitmap bitmap = BitmapConverter.ToBitmap(matSrc);
                //开始计时计算效率
                DateTime beforeDT1 = System.DateTime.Now;

                // 转换颜色空间，可以转化为yuv，下面是转化为灰度图
                Cv2.CvtColor(matDataMatrixSrc, matBinary, ColorConversionCodes.BGR2GRAY);
                //这里得到的二值图matBinary作为最后检测二维码的源
                //二值化:参数3为阈值，4为大于阈值的像素灰度值，5为二值化类型                
                double dThreshould = 70;
                double dMaxVal = 235;
                Cv2.Threshold(matBinary, matBinary, dThreshould, dMaxVal, ThresholdTypes.Binary);                

                //高斯模糊-进行比较剧烈的模糊
                OpenCvSharp.Size gSize = new OpenCvSharp.Size(35, 35);
                Cv2.GaussianBlur(matDataMatrixSrc, matGaussianBlurDm, gSize, 0, 0);
                // 转换颜色空间，可以转化为yuv，下面是转化为灰度图
                Cv2.CvtColor(matGaussianBlurDm, matGaussianBlurDm, ColorConversionCodes.BGR2GRAY);

                //进行二值化，下一步针对这个二值化结果进行提取
                double dThreshould2 = 125;
                double dMaxVal2 = 235;
                Cv2.Threshold(matGaussianBlurDm, matGaussianBlurDm, dThreshould2, dMaxVal2, ThresholdTypes.Binary);

                //轮廓提取
                OpenCvSharp.Point[][] points;
                OpenCvSharp.HierarchyIndex[] hierarchyIndices;
                Cv2.FindContours(matGaussianBlurDm, out points, out hierarchyIndices, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple);
                //绘制轮廓（暂时去掉，不需要绘制多边形）
                //for(int i=0;i<hierarchyIndices.Length;i++)
                //{
                //    Cv2.DrawContours(matDataMatrixSrc, points, i, Scalar.Red, 1, LineTypes.Link8, hierarchyIndices, 0);
                //}

                //结束计时，看运行时间
                DateTime afterDT1 = System.DateTime.Now;
                TimeSpan ts = afterDT1.Subtract(beforeDT1);
                //sInfo += "[" + ts.TotalMilliseconds + "]";

                

                OpenCvSharp.Point2f center;//圆心坐标
                float radius;//园半径
                const float MaxRadius = 60; //有用的最大半径，大于这个的排除
                const float MinRadius = 15; //有用的最小半径，小于这个的排除
                //绘制矩形的左上右下顶点坐标
                OpenCvSharp.Point pt1;
                OpenCvSharp.Point pt2;
                const double dRatio = 1.2;
                //循环进行绘制
                for (int i = 0; i < points.Length; i++)
                {
                    //寻找最小外接圆，输出中心点和半径
                    Cv2.MinEnclosingCircle(points[i], out center, out radius);
                    //Cv2.DrawContours(matSrc, points, i, new Scalar(0, 0, 255), 2, LineTypes.Link8); //去掉，不需要画出多边形

                    //控制一下，防止干扰，将大于60半径的排除
                    if ((radius > MaxRadius) || (radius < MinRadius))
                        continue;
                    //绘制内接圆（暂时去掉）
                    //Cv2.Circle(matDataMatrixSrc, (int)center.X, (int)center.Y, (int)radius, Scalar.Blue, 1, LineTypes.AntiAlias); //去掉，不需要画内接圆

                    //画出矩形区域，先定义有内容区域的左上和右下坐标
                    pt1.X = (int)center.X - (int)(radius * dRatio);
                    if (pt1.X < 0)
                        pt1.X = 0;
                    pt1.Y = (int)center.Y - (int)(radius * dRatio);
                    if (pt1.Y < 0)
                        pt1.Y = 0;
                    pt2.X = (int)center.X + (int)(radius * dRatio);
                    if (pt2.X > iVideoWidth)
                        pt2.X = iVideoWidth;
                    pt2.Y = (int)center.Y + (int)(radius * dRatio);
                    if (pt2.Y > iVideoHeight)
                        pt2.Y = iVideoHeight;
                    //检查矩形框的合理性
                    if ((pt1.X >= pt2.X) || (pt1.Y >= pt2.Y))
                        continue;

                    //绘制检测到的区域的正方形
                    Cv2.Rectangle(matDataMatrixSrc, pt1, pt2, Scalar.Green, 1, LineTypes.Link8);

                    //=========================================================================//

                    //如果不需要检测二维码则退出
                    if (!bStartFindDmCode)
                        continue;

                    //建立一个区域
                    Rect rectCutout = new Rect(pt1.X, pt1.Y, (int)(radius * dRatio * 2), (int)(radius * dRatio * 2));
                    //检查区域是否超出,若超出则退出
                    if (((rectCutout.X + rectCutout.Width) > iVideoWidth) || ((rectCutout.Y + rectCutout.Height) > iVideoHeight))
                        continue;
                    //二值图matBinary作为检测二维码的源
                    Mat matCode = matBinary[rectCutout];
                    if (IsBlankMat(matCode))
                    {
                        Cv2.Rectangle(matDataMatrixSrc, pt1, pt2, Scalar.Gray, 1, LineTypes.Link8);
                        continue;
                    }

                    long dCurrentTimeStampCode = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
                    //创建对象EntityDataMatrixMat，并将对象加入到全局 ListEntityDataMatrixMats 
                    EntityDataMatrixMat eMat = new EntityDataMatrixMat();
                    eMat.TimeStampId = dCurrentTimeStampCode;
                    eMat.CenterPt = center;
                    eMat.IndexNo = i;
                    eMat.DmMat = matCode;

                    //每3帧传递一次，基本上1s 10次
                    if ((0 == (iCount % 3)) && (iCount > 0))
                        ListEntityDataMatrixMats.Add(eMat);

                    //判断是否有已经检查出的二维码内容，将其输出到视频UI上
                    if (ListEffectiveEntityDataMatrixMats != null)
                    {
                        int iEffectiveMatsCount = ListEffectiveEntityDataMatrixMats.Count;
                        if (iEffectiveMatsCount > 0)
                        {
                            EntityDataMatrixMat mLast = ListEffectiveEntityDataMatrixMats[iEffectiveMatsCount - 1];
                            //写字
                            Cv2.PutText(matDataMatrixSrc, mLast.DmCode, new OpenCvSharp.Point((int)mLast.CenterPt.X, (int)mLast.CenterPt.Y), HersheyFonts.HersheySimplex, 1, Scalar.All(255), 2, LineTypes.Link4);
                        }
                    }

                    #region 之前的比较判断逻辑，暂时注释掉
                    /*
                     * 以下注释部分内容逻辑是对每个帧的图块进行比较，但是有点影响速度被注释。
                     * 但是有另一个思路是将小车遍历地图后，搜集到所有的特诊图片，保存到本地作为一个对照表，这样直接比较图片相似度即可。
                     * 此项优化可以不使用二维码解码，预计识别速度可提高10倍左右
                    */

                    /*
                     * 这段会引起速度慢，暂时注释掉。之前的思路是在每帧都对比是否在待解码List中已经存在类似的图块，如果存在则将这些图块去掉
                     * 暂时去掉，但是基本思路问题不大
                    if ((0 == (iCount % 3)) && (iCount > 0))
                    {
                        bool bFindSame = false;
                        if (ListEntityDataMatrixMats != null)
                        {
                            if (ListEntityDataMatrixMats.Count > 0)
                            {
                                for (int j = 0; j < ListEntityDataMatrixMats.Count; j++)
                                {
                                    if (CompareMatSimilar(ListEntityDataMatrixMats[j].DmMat, eMat.DmMat))
                                    {
                                        ListEntityDataMatrixMats[j].CenterPt = eMat.CenterPt;
                                        ListEntityDataMatrixMats[j].TimeStampId = eMat.TimeStampId;
                                        ListEntityDataMatrixMats[j].IndexNo = i;
                                        bFindSame = true;
                                        break;
                                    }
                                }

                            }
                        }
                        if (!bFindSame)
                            ListEntityDataMatrixMats.Add(eMat);
                    }
                    */
                    #endregion

                    #region 保存图块文件以便分析，暂时注释掉
                    /*
                     * 每10帧进行每个图块的保存，保存到特定文件夹下，便于日志分析
                    if (0 == (i % 10))
                    {
                        long dCurrentTimeStampCode = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
                        string filepath = @"c:\temp\log_bitmap\" + dCurrentTimeStampCode.ToString() + ".bmp";
                        Bitmap bTemp = BitmapConverter.ToBitmap(matCode);
                        SaveDmBitmap(bTemp, filepath);
                    }
                    */
                    #endregion

                    #region 原先简单的直接在每帧内进行检测，已注释
                    /*
                     * 原先简单的直接在每帧内进行检测，实测会大大影响速度
                    //检测程序运行时间

                    //如果不需要检测二维码则退出
                    if (!bStartFindDmCode)
                        continue;

                    DateTime beforeDT2 = System.DateTime.Now;
                    //时间比较长的二维码检查，实测消耗4~6s(640x480),现通过算法进行寻找二维码区域剪裁后，单个识别速度大大提高
                    List<string> lstDataMatrixCode = dmtxImageDecoder.DecodeImage(BitmapConverter.ToBitmap(matCode));
                    if (lstDataMatrixCode.Count > 0)
                    {
                        bIsFindDmCode = true;
                        foreach (string s in lstDataMatrixCode)
                        {
                            sInfo += " : " + s;
                        }

                    }
                    DateTime afterDT2 = System.DateTime.Now;
                    TimeSpan ts2 = afterDT2.Subtract(beforeDT2);

                    sInfo += "(" + ts2.TotalMilliseconds + ")";

                    if (bIsFindDmCode)
                        break;
                    */
                    #endregion
                }

                //将画上框框的结果
                //if (!bDetectLines) //如果不检测线条斜率才画，节约1次Convert
                //    bitmap = BitmapConverter.ToBitmap(matDataMatrixSrc);
                bitmap = BitmapConverter.ToBitmap(matDataMatrixSrc);

                //如果需要检测二维码则调用
                if (bStartFindDmCode)
                {
                    if ((0 == (iCount % 6)) && (iCount > 0))
                    {
                        InvokeDecodeDmCode iddc = new InvokeDecodeDmCode(DecodeDmCode);
                        BeginInvoke(iddc);
                    }
                }

                /*==============================================================================*/
                /*==============================================================================*/
                /*==============================================================================*/

                //如果不需要检测线条则不执行下面的代码
                if (!bDetectLines)
                    continue;

                /*
                // 转换颜色空间，可以转化为yuv，下面是转化为灰度图
                Cv2.CvtColor(matSrc, matGaussianBlur, ColorConversionCodes.BGR2GRAY); //原命名matGray，先改为matGaussianBlur节约一个

                //// v1原来简单二值化的内容
                ////二值化:参数3为阈值，4为大于阈值的像素灰度值，5为二值化类型
                ////threshold(lenaColor, lenaBinary, 145, 225, THRESH_BINARY);//原来的阈值设定                
                //double dThreshould = 70;
                //double dMaxVal = 235;
                //Cv2.Threshold(matGray, dst, dThreshould, dMaxVal, ThresholdTypes.Binary);
                //Bitmap bitmap = BitmapConverter.ToBitmap(dst);

                //高斯模糊
                OpenCvSharp.Size gSizeGaussianBlur = new OpenCvSharp.Size(5, 5);
                Cv2.GaussianBlur(matGaussianBlur, matGaussianBlur, gSizeGaussianBlur, 0, 0);
                //边缘检测，设置好灰度范围
                int iThreshouldLow = 100;
                int iThreshouldHigh = 225;
                Cv2.Canny(matGaussianBlur, matCanny, iThreshouldLow, iThreshouldHigh);
                //霍夫变换
                //List<Vec4i> lines = new List<Vec4i>();                
                int rho = 1;
                int threshold = 30;
                double theta = Cv2.PI / 180;
                int min_line_len = 50;
                int max_line_gap = 20;
                LineSegmentPoint[] linepoints = Cv2.HoughLinesP(matCanny, rho, theta, threshold, min_line_len, max_line_gap);
                ////简单的车道线拟合
                matHoughLines = Mat.Zeros(matCanny.Size(), MatType.CV_8UC3);

                //原来的 bitmap = BitmapConverter.ToBitmap(matSrc); 改为画了DataMatrix码框的Source
                //bitmap = BitmapConverter.ToBitmap(matSrc);
                bitmap = BitmapConverter.ToBitmap(matDataMatrixSrc);
                Graphics objGraph = Graphics.FromImage(bitmap);
                ////遍历二维直线数组 -- 原调试用
                //for (int i = 0; i < linepoints.Length; i++)
                //{
                //    objGraph.DrawLine(new Pen(Color.Red), new System.Drawing.Point(linepoints[i].P1.X, linepoints[i].P1.Y), new System.Drawing.Point(linepoints[i].P2.X, linepoints[i].P2.Y));
                //}
                //List<int> iTopLengthLines = new List<int>();

                List<LineSegmentPoint> listLineSegmentPoints = new List<LineSegmentPoint>();
                if (linepoints.Length == 0) //如果没有找到，则退出
                {
                    if (bShowPic)
                    {
                        Invalidate();
                        pbCameraView.Invalidate();
                        imgshow = BitmapConverter.ToBitmap(matSrc);
                    }
                    continue;
                }

                int[] iTopLengthLines = new int[linepoints.Length];
                //遍历二维直线数组
                for (int i = 0; i < linepoints.Length; i++)
                {
                    double dDeltaX = Math.Abs(linepoints[i].P1.X - linepoints[i].P2.X);
                    if (dDeltaX == 0)
                        dDeltaX = 0.0001;
                    double dDeltaY = Math.Abs(linepoints[i].P1.Y - linepoints[i].P2.Y);
                    if (dDeltaY == 0)
                        dDeltaY = 0.0001;
                    double dSlope = dDeltaY / dDeltaX;

                    if (dSlope > 1) //斜率大于1，是y轴方向的
                    {
                        int iLength = Math.Abs(linepoints[i].P1.X - linepoints[i].P2.X) + Math.Abs(linepoints[i].P1.Y - linepoints[i].P2.Y);
                        iTopLengthLines[i] = iLength;

                        listLineSegmentPoints.Add(linepoints[i]);
                    }
                    //objGraph.DrawLine(new Pen(Color.Red), new System.Drawing.Point(linepoints[i].P1.X, linepoints[i].P1.Y), new System.Drawing.Point(linepoints[i].P2.X, linepoints[i].P2.Y));
                }

                
                int iStartX = 0;
                int iStartY = 0;
                int iEndX = 0;
                int iEndY = 0;

                CommonUtils.BubbleSort(iTopLengthLines, ComparerFactory.GetIntComparer());
                int iTop = 2; //取最长的n条线
                if (listLineSegmentPoints.Count < iTop) //如果垂直方向上没有n条线，则退出继续循环
                    continue;

                if (iTop > linepoints.Length)
                    iTop = linepoints.Length;
                foreach (LineSegmentPoint sline in listLineSegmentPoints)
                {
                    int iLength = Math.Abs(sline.P1.X - sline.P2.X) + Math.Abs(sline.P1.Y - sline.P2.Y);
                    for (int j = linepoints.Length; j > (linepoints.Length - iTop); j--)
                    {
                        //等于最长的n条线长时，画线
                        if (iTopLengthLines[j - 1] == iLength)
                        {
                            objGraph.DrawLine(new Pen(Color.Red, 2), new System.Drawing.Point(sline.P1.X, sline.P1.Y), new System.Drawing.Point(sline.P2.X, sline.P2.Y));
                            //计算最长n条线的平均位置
                            if (sline.P1.Y > sline.P2.Y)
                            {
                                iStartX += sline.P1.X;
                                iStartY += sline.P1.Y;

                                iEndX += sline.P2.X;
                                iEndY += sline.P2.Y;
                            }
                            else
                            {
                                iStartX += sline.P2.X;
                                iStartY += sline.P2.Y;

                                iEndX += sline.P1.X;
                                iEndY += sline.P1.Y;
                            }
                        }
                    }
                }

                iStartX = iStartX / iTop;
                iStartY = iStartY / iTop;
                iEndX = iEndX / iTop;
                iEndY = iEndY / iTop;

                bool bIsSkip = false;
                int iDistanceX = 0;//x坐标距离：如果几个点的差异过大可以抛弃
                const int iThresholdDeltaX = 60; //x差异的阈值，跟线宽相关，去掉干扰点
                foreach (LineSegmentPoint sline in listLineSegmentPoints)
                {
                    if (sline.P1.Y > sline.P2.Y)
                        iDistanceX = Math.Abs(iStartX - sline.P1.X);
                    else
                        iDistanceX = Math.Abs(iStartX - sline.P2.X);

                    if (iDistanceX > iThresholdDeltaX)
                    {
                        bIsSkip = true;
                        break;
                    }
                }
                if (bIsSkip)
                    continue;

                objGraph.DrawLine(new Pen(Color.Green, 5), new System.Drawing.Point(iStartX, iStartY), new System.Drawing.Point(iEndX, iEndY));

                /*
                int iDeltaX = iStartX - iEndX;
                int iDeltaY = iStartY - iEndY;
                //下面开始计算角度（弧度）
                double dArc = Math.Atan2((double)iDeltaY, (double)iDeltaX);
                double dAngle = dArc * 180 / Math.PI;
                string sInfo = string.Format("{0:d3}", iStartX) + "," + string.Format("{0:d3}", iStartY) + " : " + dAngle.ToString("N4");
                //输出角度近端xy坐标
                objGraph.DrawString(sInfo, new Font("Arial Black", 10), new SolidBrush(Color.Navy), 15, 15);

                //long dCurrentTimeStamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
                //
                int iMiddle = iVideoWidth / 2;
                int iThresholdDeltaDistance = 50;
                double iRightAngle = 89.70; //实测由于摄像头安装原因有个角度偏差，因此此处直角未必正好是90°
                double iThresholdAngle = 5.00;
                EntityDeviation eDeviation = new EntityDeviation();
                if ((Math.Abs(iStartX - iMiddle) > iThresholdDeltaDistance) ||
                        (Math.Abs(iRightAngle - dAngle) > iThresholdAngle))
                {
                    eDeviation.DevDistance = iStartX - iMiddle;
                    eDeviation.DevAngle = iRightAngle - dAngle;
                }

                string sJson = Newtonsoft.Json.JsonConvert.SerializeObject(eDeviation); //转换对象为string
                //await SendWebSocket(sJson);
                SendWebSocket(sJson);


                sInfo = "近点距离: " + eDeviation.DevDistance + ", 角度: " + eDeviation.DevAngle.ToString("N4") + " , time:" + lDeltaTime;
                objGraph.DrawString(sInfo, new Font("Arial Black", 12), new SolidBrush(Color.Red), 15, 45);
                */
                //---------------------------------------------------------//

                
                //计算耗时
                if (0 == iCount)
                    CurrentTimeStamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
                else if (0 == (iCount % 300))
                {
                    lDeltaTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000 - CurrentTimeStamp;
                    CurrentTimeStamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;

                    sInfo += " , [ " + lDeltaTime + "]";
                }
                iCount++;


                InvokeUpdateTextbox iutb = new InvokeUpdateTextbox(SetText);
                BeginInvoke(iutb, new object[] { sInfo });
                
                

                //最后，输出到图片控件 =========================================================================
                if (bShowPic)
                {
                    Invalidate();
                    pbCameraView.Invalidate();

                    imgshow = bitmap;
                }

                //Thread.Sleep(100); //降低检测频率


            }
        }

        /// <summary>
        /// 判断这个区域是不是都是灰色图片（这种需要过滤掉）
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private bool IsBlankMat(Mat m)
        {
            bool bIsBlankMat = false;

            const int iPercentage = 5; //定义判断空点的百分比阈值
            Mat r = new Mat();

            Scalar s = new Scalar(235, 235, 235);//定义三通道颜色
            Mat n = new Mat(m.Rows, m.Cols, MatType.CV_8UC1, s); //这里要用CV_8UC1，如果是载入图片用CV_8UC3，单色图片和rgb的区别
            Cv2.BitwiseXor(m, n, r); //xor运算一下，把不是235的颜色通过xor置为1

            int iNoZero = CountMatVal(r);
            int iPoints = r.Height * r.Width;

            if (((iNoZero * 100) / iPoints) <= iPercentage)
                bIsBlankMat = true;

            return bIsBlankMat;
        }

        /// <summary>
        /// 统计不是0的像素个数
        /// </summary>
        /// <param name="matSum"></param>
        /// <returns></returns>
        private static int CountMatVal(Mat matSum)
        {
            int iCountMatVal = -1;

            try
            {
                iCountMatVal = 0;

                byte bZero = 0;

                for (int i = 0; i < matSum.Height; i++)
                {
                    for (int j = 0; j < matSum.Width; j++)
                    {
                        Vec3b color = matSum.Get<Vec3b>(i, j); //new Vec3b(); 颜色通道类型 (字节的三元组)，直接视同Get泛型方法返回指定类型

                        if (color.Item0 != bZero)
                            iCountMatVal += 1;
                        if (color.Item1 != bZero)
                            iCountMatVal += 1;
                        if (color.Item2 != bZero)
                            iCountMatVal += 1;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return iCountMatVal;
        }

        private void SaveDmBitmap(Bitmap bTemp, string filepath)
        {
            System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Bmp;
            bTemp.Save(filepath, imageFormat);
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            string filepath = @"c:\temp\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Png;

            Bitmap bit = new Bitmap(pbCameraView.ClientRectangle.Width, pbCameraView.ClientRectangle.Height);
            pbCameraView.DrawToBitmap(bit, pbCameraView.ClientRectangle);
            //bit.Save(save.FileName);
            bit.Save(filepath, imageFormat);
        }

        private void btnOpenFile1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
             ofd.Filter = "Img文件(*.bmp;*.png)|*.bmp;*.png|所有文件|*.*";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string strFileName = ofd.FileName;
                //其他代码
                tbImgFile1.Text = strFileName;
                Bitmap bitmap = new Bitmap(strFileName);
                pbImg1.Image = bitmap;
            }
        }

        private void btnOpenFile2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Img文件(*.bmp;*.png)|*.bmp;*.png|所有文件|*.*";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string strFileName = ofd.FileName;
                //其他代码
                tbImgFile2.Text = strFileName;
                Bitmap bitmap = new Bitmap(strFileName);
                pbImg2.Image = bitmap;
            }
        }

        private void btnSaveText_Click(object sender, EventArgs e)
        {
            string sFile = tbImgFile1.Text;
            string sContext = "";

            string sFileSavePath = @"c:\temp\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

            if (!(System.IO.File.Exists(sFile)))
                return;

            Mat m = new Mat(sFile);
            //打印 100 * 100  Mat对象像素值
            for (int i = 0; i < m.Height; i++)
            {
                for (int j = 0; j < m.Width; j++)
                {
                    Vec3b color = m.Get<Vec3b>(i, j); //new Vec3b(); 颜色通道类型 (字节的三元组)，直接视同Get泛型方法返回指定类型

                    //单独获取指定通道像素
                    //color.Item0= m.Get<Vec3b>(i, j).Item0;  //R
                    //color.Item1 = m.Get<Vec3b>(i, j).Item1; //G
                    //color.Item2 = m.Get<Vec3b>(i, j).Item2; //B

                    sContext +=" [ " +(color.Item0 + " " + color.Item1 + " " + color.Item2) + " ] ";
                    //sContext += 
                }

                sContext += "\r\n";

            }

            using (System.IO.FileStream fs = new System.IO.FileStream(sFileSavePath,System.IO.FileMode.OpenOrCreate,System.IO.FileAccess.Write))
            {
                byte[] buffer = Encoding.Default.GetBytes(sContext);
                fs.Write(buffer, 0, buffer.Length);
            }

        }

        private void btnCountBlack_Click(object sender, EventArgs e)
        {
            

            string sFile = tbImgFile1.Text;
            if (!(System.IO.File.Exists(sFile)))
                return;
            Mat r = new Mat();
            Mat m = new Mat(sFile);

            DateTime beforeDT = System.DateTime.Now;

            Scalar s = new Scalar(235, 235, 235);//定义三通道颜色
            Mat n = new Mat(m.Rows, m.Cols, MatType.CV_8UC3, s);
            Cv2.BitwiseXor(m, n, r); //xor运算一下，把不是235的颜色通过xor置为1

            /*
            //保存日志
            string sContext = "";
            string sFileSavePath = @"c:\temp\xor" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            //打印 100 * 100  Mat对象像素值
            for (int i = 0; i < r.Height; i++)
            {
                for (int j = 0; j < r.Width; j++)
                {
                    Vec3b color = r.Get<Vec3b>(i, j); //new Vec3b(); 颜色通道类型 (字节的三元组)，直接视同Get泛型方法返回指定类型
                    sContext += " [ " + (color.Item0 + " " + color.Item1 + " " + color.Item2) + " ] ";
                }

                sContext += "\r\n";

            }
            using (System.IO.FileStream fs = new System.IO.FileStream(sFileSavePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
            {
                byte[] buffer = Encoding.Default.GetBytes(sContext);
                fs.Write(buffer, 0, buffer.Length);
            }
            */
            
            int iNoZero = CountMatVal(r);
            int iPoints = r.Height * r.Width;
            const int iPercentage = 5;

            bool bIsBlank = false;
            if (((iNoZero * 100) / iPoints) <= iPercentage)
                bIsBlank = true;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforeDT);
            //

            tbResult.Text = "iNoZero = " + iNoZero.ToString() + " , " + "iPoints = " + iPoints.ToString() + " : " + ((iNoZero * 100) / iPoints).ToString() + "%";

            if (bIsBlank)
                tbResult.Text += " Blank img.";

            tbResult.Text += "[" + ts.TotalMilliseconds + "]";
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            string sFile1 = tbImgFile1.Text;
            string sFile2 = tbImgFile2.Text;

            if (!(System.IO.File.Exists(sFile1)))
                return;
            if (!(System.IO.File.Exists(sFile2)))
                return;

            
            Mat m = new Mat(sFile1);
            Mat n = new Mat(sFile2);

            int Xm = 0;
            int Ym = 0;
            int Xn = 0;
            int Yn = 0;

            int iWidth = m.Width; //暂默认m宽度为待截取区域宽度
            if (m.Width > n.Width)
            {
                //m宽了
                iWidth = n.Width;
                Xm = (m.Width - n.Width) / 2; //m宽了，偏移m
            }
            else
                Xn = (n.Width - m.Width) / 2;

            int iHeight = m.Height; //暂默认n高度为待截取区域高度
            if (m.Height > n.Height)
            {
                //m高了
                iHeight = n.Height;
                Ym = (m.Height - n.Height) / 2; //n高了,偏移m
            }
            else
                Yn = (n.Height - m.Height) / 2;

            //建立m区域
            Rect rectCutoutM = new Rect(Xm, Ym, iWidth, iHeight);
            Mat matCompareM = m[rectCutoutM]; //裁剪
            //建立n区域
            Rect rectCutoutN = new Rect(Xn, Yn, iWidth, iHeight);
            Mat matCompareN = n[rectCutoutN]; //裁剪

            Scalar s = new Scalar(0, 0, 0);//定义三通道颜色
            Mat r = new Mat(m.Rows, m.Cols, MatType.CV_8UC3, s);

            DateTime beforeDT = System.DateTime.Now;
            
            Cv2.BitwiseXor(matCompareM, matCompareN, r); //xor运算一下，把不同的颜色通过xor置为1

            int iNoZero = CountMatVal(r);
            int iPoints = r.Height * r.Width;
            const int iPercentage = 5;

            bool bIsBlank = false;
            if (((iNoZero * 100) / iPoints) <= iPercentage)
                bIsBlank = true;

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforeDT);
            //

            tbResult.Text = "iNoZero = " + iNoZero.ToString() + " , " + "iPoints = " + iPoints.ToString() + " : " + ((iNoZero * 100) / iPoints).ToString() + "%";

            if (bIsBlank)
                tbResult.Text += " Blank img.";

            tbResult.Text += "[" + ts.TotalMilliseconds + "]";
        }

        
    }
}
