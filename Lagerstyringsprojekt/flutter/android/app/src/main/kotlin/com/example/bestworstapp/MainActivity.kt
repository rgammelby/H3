package com.example.bestworstapp

import android.content.Intent
import android.database.Cursor
import android.media.MediaPlayer
import android.net.Uri
import android.os.Bundle
import android.provider.MediaStore
import androidx.annotation.NonNull
import io.flutter.embedding.android.FlutterActivity
import io.flutter.embedding.engine.FlutterEngine
import io.flutter.plugin.common.MethodChannel

class MainActivity: FlutterActivity() {
    private val AUDIO_CHANNEL = "com.example.bestworstapp/audio"
    private val IMAGE_PICKER_CHANNEL = "com.example.bestworstapp/image_picker"
    private var bgmPlayer: MediaPlayer? = null
    private var pendingImageResult: MethodChannel.Result? = null
    private val IMAGE_PICKER_REQUEST = 1001

    override fun configureFlutterEngine(@NonNull flutterEngine: FlutterEngine) {
        super.configureFlutterEngine(flutterEngine)

        // Audio-related platform channel
        MethodChannel(flutterEngine.dartExecutor.binaryMessenger, AUDIO_CHANNEL).setMethodCallHandler { call, result ->
            when (call.method) {
                "playSound" -> {
                    playSound()
                    result.success(null)
                }
                "playBGM" -> {
                    val success = playBGM()
                    if (success) result.success(null) else result.error("PLAY_BGM_ERROR", "Failed to play BGM", null)
                }
                "stopBGM" -> {
                    stopBGM()
                    result.success(null)
                }
                else -> result.notImplemented()
            }
        }

        // Image picker platform channel
        MethodChannel(flutterEngine.dartExecutor.binaryMessenger, IMAGE_PICKER_CHANNEL).setMethodCallHandler { call, result ->
            when (call.method) {
                "pickImage" -> {
                    pendingImageResult = result
                    openImagePicker()
                }
                else -> result.notImplemented()
            }
        }
    }

    override fun onResume() {
        super.onResume()
        playBGM()
    }

    private fun playSound() {
        val mediaPlayer = MediaPlayer.create(this, R.raw.activated)
        mediaPlayer.start()
        mediaPlayer.setOnCompletionListener {
            it.release()
        }
    }

    private fun playBGM(): Boolean {
        return try {
            if (bgmPlayer == null) {
                bgmPlayer = MediaPlayer.create(this, R.raw.bgm)
                bgmPlayer?.isLooping = true
                bgmPlayer?.start()
            }
            true
        } catch (e: Exception) {
            e.printStackTrace()
            false
        }
    }

    private fun stopBGM() {
        bgmPlayer?.let {
            if (it.isPlaying) {
                it.stop()
                it.release()
                bgmPlayer = null
            }
        }
    }

    private fun openImagePicker() {
        val intent = Intent(Intent.ACTION_PICK, MediaStore.Images.Media.EXTERNAL_CONTENT_URI)
        startActivityForResult(intent, IMAGE_PICKER_REQUEST)
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == IMAGE_PICKER_REQUEST && resultCode == RESULT_OK && data != null) {
            val selectedImage: Uri = data.data
            val imagePath: String? = getImagePath(selectedImage)
            pendingImageResult?.success(imagePath)
            pendingImageResult = null
        } else if (pendingImageResult != null) {
            pendingImageResult?.success(null)
            pendingImageResult = null
        }
    }

    private fun getImagePath(uri: Uri?): String? {
        uri ?: return null
        val projection = arrayOf(MediaStore.Images.Media.DATA)
        val cursor: Cursor = contentResolver.query(uri, projection, null, null, null)
        cursor?.use {
            if (it.moveToFirst()) {
                val columnIndex = it.getColumnIndexOrThrow(MediaStore.Images.Media.DATA)
                return it.getString(columnIndex)
            }
        }
        return uri.path
    }

    override fun onDestroy() {
        super.onDestroy()
        bgmPlayer?.release()
        bgmPlayer = null
    }
}
