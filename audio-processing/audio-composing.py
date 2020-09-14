
from moviepy.editor import *
videoclip = VideoFileClip("output.avi")
audioclip = AudioFileClip("audio.mp3")

new_audioclip = CompositeAudioClip([audioclip])
videoclip.audio = new_audioclip
videoclip.write_videofile("output2.mp4")