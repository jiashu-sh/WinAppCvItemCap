
/*
                // * 失败的抠图
                Scalar scalarMaskLower = new Scalar(35, 43, 46);
                Scalar scalarMaskUpper = new Scalar(77, 255, 255); Scalar scalarMaskUpper = new Scalar(155, 255, 255);

                //转换颜色空间为hsv
                Cv2.CvtColor(matSrc, matHsv, ColorConversionCodes.BGR2HSV);
                Cv2.InRange(matHsv, scalarMaskLower, scalarMaskUpper, matMask);//颜色过滤

                //进行边缘多边形 形态学变换 morphologyEx
                Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
                Cv2.MorphologyEx(matMask, matMask, MorphTypes.Dilate, kernel);

                //高斯模糊
                OpenCvSharp.Size gSize3x3 = new OpenCvSharp.Size(3, 3);
                Cv2.GaussianBlur(matMask, matGaussianBlur, gSize3x3, 0, 0);

                //Cv2.CvtColor(matGaussianBlur, matMask, ColorConversionCodes.RGBA2GRAY);
                //Cv2.CvtColor(matMask, matMask, ColorConversionCodes.RGBA2GRAY);

                matSrc.CopyTo(matShow, matMask);
                Bitmap bitmap = BitmapConverter.ToBitmap(matShow);
                */

