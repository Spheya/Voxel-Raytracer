import time
import cv2

# import image module from pillow
from PIL import Image, ImageDraw
base = r"C:\Users\mikel\Desktop\Proftaak 2.0\foto's\\"
foto = "Winners.png"
mike = "Mike.png"
basefoto = base+foto
mikefoto = base+mike

# Image1=cv2.imread(basefoto)
# Image2=cv2.imread(mikefoto)
# open the image

# Image1 = cv2.circle(Image1, (64, 64), 30, color, thickness)
Image1 = Image.open(mikefoto)
Image2 = Image.open(basefoto)
newsize = (200, 200)
Image1 = Image1.resize(newsize)

Image1copy = Image1.copy()
Image2copy = Image2.copy()

# Image1copy.paste(Image2copy, (0, 0))
# create grayscale image with white circle (255) on black background (0)
mask = Image.new('L', Image1copy.size)
mask_draw = ImageDraw.Draw(mask)
breedte, lengte = Image1copy.size
mask_draw.ellipse((0, 0, breedte, lengte), fill=255)
#mask.show()

# add mask as alpha channel
Image1copy.putalpha(mask)
# Image1copy.save("new.png")
# newbase = base+"new.png"
# save as png which keeps alpha channel
width, height = Image2copy.size
# Image1copy.save('dog_circle.png')
# time.sleep(1)
# print(width, height)
pastewidth = width*0.4525
pasteheight = height*0.2721617418351477
Image2copy.paste(Image1copy, (int(pastewidth), int(pasteheight)), mask)
Image2copy.show()
# Image1copy.show()
# save the image

# # save the image
# cv2.imshow("photo", a)
# cv2.waitKey(0)
# cv2.destroyAllWindows()
