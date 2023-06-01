const fs = require('fs')
const path = require('path')
const intro_images_dir_path = path.join(__dirname, 'storage/intro_images')
const intro_images_text_dir_path = path.join(__dirname, 'storage/intro_images_text')
const { exec } = require('child_process');


function cropImageToIphoneSize(image) {
    const imagename = image.split('.')[0]
    const imagePath = intro_images_dir_path + "/" +image
    const outputImagePath = intro_images_dir_path + `/output_${imagename}.jpeg`
    const command = `ffmpeg -i ${imagePath} -vf "scale=1300:2800:force_original_aspect_ratio=increase,crop=1300:2800" -qscale:v 1 ${outputImagePath}`;
    executeFfmpegCommand(command);
}

function createTextFile(text) {
    fs.writeFile(intro_images_text_dir_path + '/text.txt', text, (err) => {
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
createTextFile("afasf\nasfasfas\nasfdasd")
// fs.readdir(intro_images_dir_path, (error, files) => {
//   if (error) console.log(error);
//   files.forEach((file) => {
//     cropImageToIphoneSize(file)
//   })
// });

