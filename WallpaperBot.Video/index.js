//for linux
//writtein in linux wsl subsystem
// should work in docker copium
const util = require("util");
const fs = require("fs");
const path = require("path");
const intro_images_dir_path = path.join(__dirname, "storage/intro_images");
const intro_video_output_dir = path.join(__dirname, "storage/pipe/intro");
const slideshow_video_output_dir = path.join(__dirname, "storage/pipe/slideshow");
const slideshow_wallpapers_dir = path.join(__dirname, "storage/wallpapers");
const { exec } = require("child_process");

const intro_images_text_dir_path = path.join(
    __dirname,
    "storage/intro_images_text"
);

async function cropImageToIphoneSize(image, dir) {
    const imagename = image.split(".")[0];
    const imagePath = dir + "/" + image;
    const outputImagePath = dir + `/cropped/cropped_${imagename}.jpeg`;
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

async function writeTextToImage(image, textFile) {
    const imagename = image.split(".")[0];
    const croppedImagePath =
        intro_images_dir_path + `/cropped/` + `cropped_${imagename}.jpeg`;
    const textFilePath = intro_images_text_dir_path + `/` + `${textFile}`;
    const command = `ffmpeg -i ${croppedImagePath} -vf "drawtext=textfile=${textFilePath}:fontsize=0.1*W:fontcolor=white:x=0.1*W:y=0.3*H:bordercolor=black:borderw=15" -q:v 1 ${intro_images_dir_path}/with_text/final_${imagename}.jpeg`;
    await executeFfmpegCommand(command);
}

async function duplicateImageOverlay(image) {
    const imagename = image.split(".")[0];
    const inputPath =
        slideshow_wallpapers_dir + `/cropped` + `/cropped_${imagename}.jpeg`;
    const outputImagePath =
        slideshow_wallpapers_dir + `/final` + `/${imagename}.jpeg`;
    const command = `ffmpeg -i ${inputPath} -i ${inputPath} -filter_complex "[0:v]eq=gamma=1.7[gamma_corrected]; [1:v]scale=iw*0.8:ih*0.8[scaled_overlay]; [gamma_corrected][scaled_overlay]overlay=(main_w-overlay_w)/2:(main_h-overlay_h)/2" -qscale:v 2 ${outputImagePath}`;
    await executeFfmpegCommand(command);
}

//image or single images
async function convertImageToVideo(imagePath, images, outputPath, outputName) {
    const output = outputPath + "/video/" + outputName;
    const command = `ffmpeg -loop 1 -i ${imagePath + "/" + images} -c:v libx264 -t 2 -pix_fmt yuv422p ${output}.mp4`
    await executeFfmpegCommand(command);
}

async function imagesToSlideshowVideo(imagesFileFormat) {
    // image%d - format: image[number]
    console.log("Rendering slideshow...")
    const inputPath = slideshow_wallpapers_dir + `/final`
    const randomName = (Math.random() + 1).toString(36).substring(7);
    const command = `ffmpeg -r 1/2 -pattern_type glob -i '${inputPath}/*.jpeg' -r 25 -c:v libx264 -pix_fmt yuv422p ${slideshow_video_output_dir}/slideshow_${randomName}.mp4`
    await executeFfmpegCommand(command);
}

async function introPipeOne() {
    const textFiles = await fs.promises.readdir(intro_images_text_dir_path);
    const files = await fs.promises.readdir(intro_images_dir_path);

    for (const file of files) {
        if (file.split(".").length > 1) {
            try {
                const randomTextFile =
                    textFiles[Math.floor(Math.random() * textFiles.length)];
                await cropImageToIphoneSize(file, intro_images_dir_path);
                await writeTextToImage(file, randomTextFile);
            } catch (e) {
                console.error(e);
                throw e;
            }
        }
    }
}

async function introPipeTwo() {
    const inputPath = intro_images_dir_path + "/with_text";
    const files = await fs.promises.readdir(inputPath);

    for (const file of files) {
        const name = file.split(".")[0];
        if (file.split(".").length > 1) {
            try {
                await convertImageToVideo(
                    inputPath,
                    file,
                    intro_video_output_dir,
                    "video_" + name
                );
            } catch (e) {
                console.error(e);
                throw e;
            }
        }
    }
}

async function introPipeline() {
    await introPipeOne();
    await introPipeTwo();
    console.log("intro pipeline completed");
}

async function slideshowPipeOne() {
    const files = await fs.promises.readdir(slideshow_wallpapers_dir);
    for (const file of files) {
        if (file.split(".").length > 1) {
            console.log(file);
            try {
                await cropImageToIphoneSize(file, slideshow_wallpapers_dir);
                await duplicateImageOverlay(file);
            } catch (e) {
                console.err(e);
            }
        }
    }
}

async function combineIntroSlideshow() {

}

async function run() {
    // await slideshowPipeOne();
    // await imagesToSlideshowVideo("overlayedimage%03d")
    await introPipeline();
}
// need to fix or just add -y  flag to ffmpeg commands
// to remove bug, if output image already exists, it waits for user command(freezezs theexecution ;9)

(async () => {
    try {
        const text = await run();
    } catch (e) {
        // Deal with the fact the chain failed
    }
    // `text` is not available here
})();
// duplicateImageOverlay("image4.jpeg")
