import 'dart:async';
import 'package:flutter/services.dart';

class NativeAudio {
  static const MethodChannel _channel = MethodChannel('com.example.bestworstapp/audio');

  static Future<void> playSound() async {
    try {
      await _channel.invokeMethod('playSound');
    } on PlatformException catch (e) {
      print("Failed to play sound: '${e.message}'.");
    }
  }

  // Method to start playing background music (BGM) on loop
  static Future<void> playBGM() async {
    try {
      await _channel.invokeMethod('playBGM');
    } on PlatformException catch (e) {
      print("Failed to play BGM: '${e.message}'.");
    }
  }

  static Future<void> stopBGM() async {
    try {
      await _channel.invokeMethod('stopBGM');
    } on PlatformException catch (e) {
      print("Failed to stop BGM: '${e.message}'.");
    }
  }
}
