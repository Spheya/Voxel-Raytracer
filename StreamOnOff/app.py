from flask import Flask, render_template, Response, request
import cv2
import numpy as np


app = Flask(__name__)
camera = cv2.VideoCapture(0)  # use 0 for web camera

#  for cctv camera use rtsp://username:password@ip_address:554/user=username_password='password'_channel=channel_number_stream=0.sdp' instead of camera
state="aan"
def gen_frames():  # generate frame by frame from camera
    while True:
        success, frame = camera.read()  # read the camera frame
        # frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        # ret,frame = cv2.threshold(frame,127,255,cv2.THRESH_BINARY)
        if state == "uit":
            frame = np.zeros(shape=[512, 512, 3], dtype=np.uint8)
        if not success:
            break
        else:
            ret, buffer = cv2.imencode('.jpg', frame)
            frame = buffer.tobytes()
            yield b'--frame\r\n Content-Type: image/jpeg\r\n\r\n' + frame + b'\r\n'


@app.route('/video_feed')
def video_feed():
    return Response(gen_frames(), mimetype='multipart/x-mixed-replace; boundary=frame')


@app.route('/admin')
def admin():
    global state
    state = request.args.get('state')
    return '''<a href="http://localhost/admin?state=aan"><button>aan</button></a><a href="http://localhost/admin?state=uit"><button>uit</button></a><style>button{height:50vh;width:50vh;}</style>'''


@app.route('/')
def index():
    return render_template('index.html')


if __name__ == '__main__':
    app.run(port=80, host="localhost")
