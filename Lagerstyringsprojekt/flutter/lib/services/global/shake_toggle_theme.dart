import 'dart:async';
import 'dart:math';
import 'package:sensors_plus/sensors_plus.dart';

class ShakeToggleTheme {
  static const double _shakeThreshold = 40.0; // Sensitivity for shake detection
  static const int _debounceDuration = 1000; // Debounce time in milliseconds

  Function onShakeDetected;
  StreamSubscription? _subscription;
  bool _isShaking = false;
  DateTime _lastShakeTime = DateTime.now();
  bool isShakeEnabled;

  ShakeToggleTheme({required this.onShakeDetected, this.isShakeEnabled = true});

  // Enable or disable shake detection
  void setShakeEnabled(bool enabled) {
    isShakeEnabled = enabled;
  }

  // Start listening for shake events
  void startListening() {
    _subscription = accelerometerEvents.listen((AccelerometerEvent event) {
      final double x = event.x;
      final double y = event.y;
      final double z = event.z;

      // Calculate the magnitude of acceleration
      final double magnitude = sqrt(x * x + y * y + z * z);

      if (isShakeEnabled && magnitude > _shakeThreshold) {
        final DateTime now = DateTime.now();

        // Debounce logic to prevent excessive triggering
        if (!_isShaking && now.difference(_lastShakeTime).inMilliseconds > _debounceDuration) {
          _isShaking = true;
          _lastShakeTime = now;
          onShakeDetected();
        }
      } else {
        // Reset `_isShaking` when the magnitude falls below the threshold
        _isShaking = false;
      }
    });
  }

  // Stop listening for shake events
  void stopListening() {
    _subscription?.cancel();
  }
}
