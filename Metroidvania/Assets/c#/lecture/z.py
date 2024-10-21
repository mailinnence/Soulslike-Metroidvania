import os
import cv2

def extract_frames(video_path, output_folder):
    # 동영상 파일 열기
    video_capture = cv2.VideoCapture(video_path)
    frame_count = 0

    while True:
        ret, frame = video_capture.read()
        if not ret:
            break
        frame_filename = f"{output_folder}/frame_{frame_count}.jpg"
        cv2.imwrite(frame_filename, frame)
        frame_count += 1

    video_capture.release()


video_path = 'C:\\Users\\mailinnence\\Desktop\\mv\\a.mp4'


output_folder = 'C:\\Users\\mailinnence\\Desktop\\mv\\output_frames'


os.makedirs(output_folder, exist_ok=True)

extract_frames(video_path, output_folder)



'''
점프 공격 >> 애니메이션으로 변환
acting 으로 인헤 데미지 받았을때와 점프공격에서 현재 행동이 안되고 있음 로직 바꿔서
가능하게 할것
콤보 

그러면 내려찍기와 찌르기만 만들면 주인공 공격은 끝

'''