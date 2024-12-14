#!/usr/bin/python3
import os
import random
import xml.etree.ElementTree as ET
from os import getcwd
from pathlib import Path
import glob
import shutil

# 配置参数
TRAIN_PERCENT = 0.8                     # train占总数据的比例
cfg_path = "cfg/yolov4-tiny.cfg"     # 模板文件的路径
batch = 64
subdivisions = 1                       # 根据显存大小设置
width = 608
height = 608                            # 32的倍数{320，352…..608}, 最小320*320，最大608*608, 影响precision

# 只保留 train 和 trainval 数据集
sets = ['train', 'trainval']  
classes = []

# 数据集路径
BASE_DIR = "VOCdevkit"
ANNOTATIONS_DIR = os.path.join(BASE_DIR, "Annotations")  # 标注文件所在目录
IMAGESETS_DIR = os.path.join(BASE_DIR, "ImageSets")  # 存放图像集的目录
LABELS_DIR = os.path.join(BASE_DIR, "labels")  # 存放标签文件的目录
IMAGES_DIR = os.path.join(BASE_DIR, "JPEGImages")  # 图像文件所在目录
TRAIN_DIR = os.path.join(BASE_DIR, "Train")  # 训练相关的文件夹
WEIGHTS_DIR = os.path.join(TRAIN_DIR, "Weights")  # 权重文件夹路径

# 确保各个目录存在
os.makedirs(ANNOTATIONS_DIR, exist_ok=True)
os.makedirs(IMAGESETS_DIR, exist_ok=True)
os.makedirs(LABELS_DIR, exist_ok=True)
os.makedirs(IMAGES_DIR, exist_ok=True)
os.makedirs(TRAIN_DIR, exist_ok=True)
os.makedirs(WEIGHTS_DIR, exist_ok=True)

# 获取所有 XML 文件
total_xml = [f[:-4] for f in os.listdir(ANNOTATIONS_DIR) if f.endswith(".xml")]
num = len(total_xml)  # 总数据量

# 计算划分数量
train_count = int(num * TRAIN_PERCENT)  # 根据设置的比例分配给train

# 随机选择数据
random.seed(42)  # 保证结果可重复
train = set(random.sample(total_xml, train_count))  # train集合
trainval = set(total_xml) - train  # 剩余数据分配给 trainval

# 输出调试信息
train_percent = len(train) / num * 100
trainval_percent = len(trainval) / num * 100

print(f"总数据量: {num}")
print(f"Train数量: {len(train)} ({train_percent:.2f}%)")
print(f"TrainVal数量: {len(trainval)} ({trainval_percent:.2f}%)")


# 保存划分结果
splits = {
    "train": train,
    "trainval": trainval
}

# 保存每个集合的图片文件名
for split, dataset in splits.items():
    with open(os.path.join(IMAGESETS_DIR, f"{split}.txt"), "w") as f:
        f.writelines(f"{name}\n" for name in dataset)
    print(f"{os.path.join(IMAGESETS_DIR, f'{split}.txt')} 文件已生成")

def gen_classes(image_id, classes):
    """
    解析指定图像的XML文件，提取类别信息并更新类列表
    """
    in_file = open(os.path.join(ANNOTATIONS_DIR, f'{image_id}.xml'))
    tree = ET.parse(in_file)
    root = tree.getroot()

    # 遍历 XML 文件中的 'object' 元素，提取类别名称
    for obj in root.iter('object'):
        cls_name = obj.find('name').text
        if cls_name not in classes:
            classes.append(cls_name)  # 将新类别添加到 classes 列表
    in_file.close()
    return classes


def convert(size, box):
    """
    将边界框的坐标转换为相对比例的形式，适应YOLO格式
    """
    dw = 1. / (size[0])  # 图像宽度的归一化因子
    dh = 1. / (size[1])  # 图像高度的归一化因子
    x = (box[0] + box[1]) / 2.0 - 1  # 中心点的X坐标
    y = (box[2] + box[3]) / 2.0 - 1  # 中心点的Y坐标
    w = box[1] - box[0]  # 宽度
    h = box[3] - box[2]  # 高度
    x = x * dw  # 中心X坐标归一化
    w = w * dw  # 宽度归一化
    y = y * dh  # 中心Y坐标归一化
    h = h * dh  # 高度归一化
    return (x, y, w, h)


def convert_annotation(image_id):
    """
    处理每个图像的 XML 文件，将其转换为 YOLO 格式的标签文件
    """
    # 打开 XML 文件进行解析
    in_file = open(os.path.join(ANNOTATIONS_DIR, f'{image_id}.xml'))
    out_file = open(os.path.join(LABELS_DIR, f'{image_id}.txt'), 'w')
    
    tree = ET.parse(in_file)
    root = tree.getroot()
    size = root.find('size')  # 获取图像尺寸信息
    w = int(size.find('width').text)  # 图像宽度
    h = int(size.find('height').text)  # 图像高度

    # 遍历所有物体对象并转换为 YOLO 格式
    for obj in root.iter('object'):
        difficult = obj.find('difficult').text  # 检查物体是否困难
        cls = obj.find('name').text  # 获取类别名称
        if cls not in classes or int(difficult) == 1:
            continue  # 如果是困难物体或不在类别中则跳过
        cls_id = classes.index(cls)  # 获取类别 ID
        xmlbox = obj.find('bndbox')  # 获取物体的边界框
        b = (float(xmlbox.find('xmin').text), float(xmlbox.find('xmax').text),
             float(xmlbox.find('ymin').text), float(xmlbox.find('ymax').text))
        bb = convert((w, h), b)  # 将边界框坐标转换为相对比例
        out_file.write(f"{cls_id} {' '.join([str(a) for a in bb])}\n")  # 写入标签文件
    
    in_file.close()
    out_file.close()


# 获取当前工作目录
wd = getcwd()

# 执行 convert_annotation
for image_set in sets:
    # 打开文件写入图像路径，将文件保存在 TRAIN_DIR 下
    with open(os.path.join(TRAIN_DIR, f'{image_set}.txt'), 'w') as list_file:
        with open(os.path.join(IMAGESETS_DIR, f'{image_set}.txt')) as f:
            image_ids = f.readlines()
        
        # 遍历每个图像ID
        for image_id in image_ids:
            image_id = image_id.strip()  # 去掉两端的换行符，保留文件名中间的空格

            # 获取图像的完整路径
            image_file_path = os.path.join(IMAGES_DIR, image_id)

            # 使用 glob 查找文件（完全匹配文件名，并自动加上扩展名）
            matched_files = glob.glob(f'{image_file_path}.*')  # 直接在当前目录查找带扩展名的文件

            # 直接使用找到的第一个匹配项
            image_file_path = matched_files[0]  # 获取第一个匹配文件路径

            # 写入路径到文件
            list_file.write(f'{wd}/{image_file_path}\n')

            # 更新类别列表
            gen_classes(image_id, classes)
            
            # 执行标签转换
            convert_annotation(image_id)
    print(f"{os.path.join(TRAIN_DIR, f'{image_set}.txt')} 文件已生成")
print(f"{LABELS_DIR} 标签已生成")

# 输出 classes 列表的总数
print(f"classes = {len(classes)}")
# 输出 filters 的计算结果
print(f"filters = 3 * (5 + classes)) = {3 * (5 + len(classes))}")

# 将类别写入到 TRAIN_DIR 下的 train.names 文件
with open(os.path.join(TRAIN_DIR, 'train.names'), 'w') as f:
    for class_name in classes:
        f.write(f"{class_name}\n")

print(f"{os.path.join(TRAIN_DIR, 'train.names')} 文件已生成")

# 复制类别文件到 WEIGHTS_DIR 下
shutil.copy(os.path.join(TRAIN_DIR, 'train.names'), os.path.join(WEIGHTS_DIR, 'test.names'))

print(f"{os.path.join(WEIGHTS_DIR, 'test.names')} 文件已生成")

# 获取 TRAIN_DIR 目录的绝对路径
train_dir = Path(TRAIN_DIR).resolve()

# 自动生成文件路径
train_data_path = train_dir / "train.data"
train_txt_path = train_dir / "train.txt"
trainval_txt_path = train_dir / "trainval.txt"
names_txt_path = train_dir / "train.names"
weights_dir = train_dir / "Weights"

# 写入 train.data 文件
train_data_path.write_text(f"""\
classes= {len(classes)}
train  = {train_txt_path.resolve()}
valid  = {trainval_txt_path.resolve()}
names = {names_txt_path.resolve()}
backup = {weights_dir.resolve()}
""")

print(f"{os.path.join(TRAIN_DIR, 'train.data')} 文件已生成")

# 获取 filters 和 classes 的值
classes = len(classes)  # 计算 classes 的值
filters = 3 * (5 + classes)  # 根据前面的代码计算 filters
max_batches = max(6000, 2000 * classes)  # 每个class建议2000次, 最小值6000
steps1 = int(0.8 * max_batches)
steps2 = int(0.9 * max_batches)

# 读取模板文件
with open(cfg_path, 'r') as cfg_file:
    lines = cfg_file.readlines()
print(f"{cfg_path} 文件已读取")

# 查找包含 yolo 关键字的行号
yolo_line_numbers = []
for i, line in enumerate(lines):
    if 'yolo' in line.lower():
        yolo_line_numbers.append(i)

# 修改 filters 和 classes
if yolo_line_numbers:
    print("开始修改 yolo 关键字上下文的 filters 和 classes")
    # 对于每个 yolo 行，更新对应的 filters 和 classes
    for yolo_line_number in yolo_line_numbers:

        # 查找第一个 filters 行号，向上搜索
        filters_line_number = None
        for i in range(yolo_line_number - 1, -1, -1):
            if 'filters=' in lines[i]:
                filters_line_number = i
                break

        # 查找第一个 classes 行号，向下搜索
        classes_line_number = None
        for i in range(yolo_line_number + 1, len(lines)):
            if 'classes=' in lines[i]:
                classes_line_number = i
                break

        # 确保找到 filters 和 classes
        if filters_line_number is not None and classes_line_number is not None:
            # 更新 filters 和 classes 的值
            lines[filters_line_number] = f'filters={filters}\n'  # 更新 filters 行
            lines[classes_line_number] = f'classes={classes}\n'  # 更新 classes 行

            print(f"第 {yolo_line_number + 1} 行的 filters 已更新为 {filters}")
            print(f"第 {classes_line_number + 1} 行的 classes 已更新为 {classes}")
        else:
            print(f"第 {yolo_line_number + 1} 行的 filters 或 classes 未找到")
else:
    print("未找到 yolo 相关的行")

# 查找需要修改的参数
for i, line in enumerate(lines):
    if '# Training' in line:
        lines[i + 1] = f'batch=1\n'  # 更新 batch 行
        print(f"第 {i + 1} 行的 batch 已更新为 1")
        lines[i + 2] = f'subdivisions=1\n'  # 更新 subdivisions 行
        print(f"第 {i + 2} 行的 subdivisions 已更新为 1")
    elif 'width=' in line:
        lines[i] = f'width={width}\n'  # 更新 width 行
        print(f"第 {i + 1} 行的 width 已更新为 {width}")
    elif 'height=' in line:
        lines[i] = f'height={height}\n'  # 更新 height 行
        print(f"第 {i + 1} 行的 height 已更新为 {height}")
    elif 'max_batches =' in line:
        lines[i] = f'max_batches = {max_batches}\n'  # 更新 max_batches 行
        print(f"第 {i + 1} 行的 max_batches 已更新为 {max_batches}")
    elif 'steps=' in line:
        lines[i] = f'steps={steps1},{steps2}\n'  # 更新 steps 行
        print(f"第 {i + 1} 行的 steps 已更新为 {steps1},{steps2}")

    # 将更新后的内容写到 test_cfg_path
    with open(os.path.join(WEIGHTS_DIR, 'test.cfg'), 'w') as cfg_file:
        cfg_file.writelines(lines)
print(f"{os.path.join(WEIGHTS_DIR, 'test.cfg')} 文件已生成")

# 查找需要修改的参数
for i, line in enumerate(lines):
    if '# Training' in line:
        lines[i + 1] = f'batch={batch}\n'  # 更新 batch 行
        print(f"第 {i + 1} 行的 batch 已更新为 {batch}")
        lines[i + 2] = f'subdivisions={subdivisions}\n'  # 更新 subdivisions 行
        print(f"第 {i + 2} 行的 subdivisions 已更新为 {subdivisions}")

    # 将更新后的内容写到 train_cfg_path
    with open(os.path.join(TRAIN_DIR, 'train.cfg'), 'w') as cfg_file:
        cfg_file.writelines(lines)
print(f"{os.path.join(TRAIN_DIR, 'train.cfg')} 文件已生成")
