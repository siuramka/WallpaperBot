//for linux
//writtein in linux wsl subsystem
// should work in docker copium
const fs = require('fs')
const path = require('path')
const intro_images_dir_path = path.join(__dirname, 'storage/intro_images')
const intro_images_text_dir_path = path.join(__dirname, 'storage/intro_images_text')
const { exec } = require('child_process');


function cropImageToIphoneSize(image) {
    const imagename = image.split('.')[0]
    const imagePath = intro_images_dir_path + "/" +image
    const outputImagePath = intro_images_dir_path + `/cropped_${imagename}.jpeg`
    const command = `ffmpeg -i ${imagePath} -vf "scale=1300:2800:force_original_aspect_ratio=increase,crop=1300:2800" -qscale:v 1 ${outputImagePath}`;
    executeFfmpegCommand(command);
}

function createTextFile(text, name) {
    fs.writeFile(intro_images_text_dir_path + `/${name}.txt`, text, (err) => {
        if (err) throw err;
        console.log('Text file created!');
      });
}

function executeFfmpegCommand(command) {
    exec(command, (error, stdout, stderr) => {
        if (error) {
          console.error('Error converting image:', error);
          return;
        }
        console.log('Image conversion complete!');
      });
}

function writeTextToImage(image, textFile) {
    const imagename = image.split('.')[0]
    const croppedImagePath = intro_images_dir_path + `/` + `cropped_${imagename}.jpeg`
    const textFilePath = intro_images_text_dir_path + `/` + `${textFile}`
    const command = `ffmpeg -i ${croppedImagePath} -vf "drawtext=textfile=${textFilePath}:fontsize=0.1*W:fontcolor=white:x=0.1*W:y=0.3*H:bordercolor=black:borderw=15" -q:v 1 ${intro_images_dir_path}/final_${image}`;
    executeFfmpegCommand(command);
}
// createTextFile("text","text1")
writeTextToImage("image1.jpeg","text.txt")

// fs.readdir(intro_images_dir_path, (error, files) => {
//   if (error) console.log(error);
//   files.forEach((file) => {
//     try {
//       cropImageToIphoneSize(file);
//     } catch (e) {
//       console.error(e);
//     }
//   });
// });

