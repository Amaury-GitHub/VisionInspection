# Vision Inspection

最近开始接触AI视觉检测<br>

在公司有台电脑用于训练, ubuntu系统<br>

办公电脑是windows, 写个小工具用来测试模型的可用度<br>

方便去现场实时抓图后快速测试<br>

适配yolov3, 其他的不确定是否可用<br>

测试的配置,权重,类别文件来自<br>
[https://github.com/pjreddie/darknet](https://github.com/pjreddie/darknet)<br>
[https://pjreddie.com/darknet](https://pjreddie.com/darknet/)<br>
[https://pjreddie.com/media/files/yolov3.weights](https://pjreddie.com/media/files/yolov3.weights)


update: 增加了配置,权重,类别文件的自动加载, 增加了置信度,nms抑制的参数设置, 增加了标准文字大小的设置, 增加了日志框显示, 增加了文件夹批量检查<br><br>
update: 增加了RTSP的检查选项<br><br>
update: OpenCvSharp会断流, 各种不正常, 改用LibVLCSharp实现视频流的连接<br><br>
![image](https://github.com/Amaury-GitHub/VisionInspection/blob/master/IMG/img1.png)<br>
