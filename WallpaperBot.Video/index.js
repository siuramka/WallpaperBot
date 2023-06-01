//for linux
//writtein in linux wsl subsystem
// should work in docker copium
const fs = require("fs");
const path = require("path");
const intro_images_dir_path = path.join(__dirname, "storage/intro_images");
const intro_images_text_dir_path = path.join(
  __dirname,
  "storage/intro_images_text"
);
const { exec } = require("child_process");

async function cropImageToIphoneSize(image, dir) {
  const imagename = image.split(".")[0];
  const imagePath = dir + "/" + image;
  const outputImagePath = dir + `/cropped_${imagename}.jpeg`;
  const command = `ffmpeg -i ${imagePath} -vf "scale=1300:2800:force_original_aspect_ratio=increase,crop=1300:2800" -qscale:v 1 ${outputImagePath}`;
  await executeFfmpegCommand(command);
}

function createTextFile(text, name) {
  fs.writeFile(intro_images_text_dir_path + `/${name}.txt`, text, (err) => {
    if (err) throw err;
    console.log("Text file created!");
  });
}

function executeFfmpegCommand(command) {
  return new Promise((resolve, reject) => {
    exec(command, (error, stdout, stderr) => {
      if (error) {
        reject(error);
        console.error("Error converting image:", error);
      }
      resolve("done");
      console.log("Image conversion complete!");
    });
  });
}

function writeTextToImage(image, textFile) {
  const croppedImagePath =
    intro_images_dir_path + `/` + `cropped_${imagename}.jpeg`;
  const textFilePath = intro_images_text_dir_path + `/` + `${textFile}`;
  const command = `ffmpeg -i ${croppedImagePath} -vf "drawtext=textfile=${textFilePath}:fontsize=0.1*W:fontcolor=white:x=0.1*W:y=0.3*H:bordercolor=black:borderw=15" -q:v 1 ${intro_images_dir_path}/final_${imagename}.jpeg`;
  executeFfmpegCommand(command);
}

function duplicateImageOverlay(image) {
  const imagename = image.split(".")[0];
  const finalImagePath = wallpapers_dir + `/` + `cropped_${imagename}.jpeg`;
  const outputImagePath = wallpapers_dir + `/overlayed_${imagename}.jpeg`;
  const command = `ffmpeg -i ${finalImagePath} -i ${finalImagePath} -filter_complex "[0:v]eq=gamma=1.7[gamma_corrected]; [1:v]scale=iw*0.8:ih*0.8[scaled_overlay]; [gamma_corrected][scaled_overlay]overlay=(main_w-overlay_w)/2:(main_h-overlay_h)/2" -qscale:v 2 ${outputImagePath}`;
  executeFfmpegCommand(command);
}
const wallpapers_dir = path.join(__dirname, "storage/wallpapers");

async function run() {
    await cropImageToIphoneSize("image4.jpeg", wallpapers_dir);
    await cropImageToIphoneSize("image1.jpeg", wallpapers_dir);
}

// need to fix or just add -y  flag to ffmpeg commands 
// to remove bug, if output image already exists, it waits for user command(freezezs theexecution ;9)

run();
// duplicateImageOverlay("image4.jpeg")

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
