import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';

class ToastService {
  static void showSuccessToast(BuildContext context, String message) {
    _showToast(context, message, Colors.green);
  }

  static void showErrorToast(BuildContext context, String message) {
    _showToast(context, message, Colors.red);
  }

  static void showCustomToast(
      BuildContext context,
      String message, {
        required Color backgroundColor,
        Color textColor = Colors.white,
        ToastGravity gravity = ToastGravity.BOTTOM,
        Duration duration = const Duration(seconds: 2),
      }) {
    _showToast(
      context,
      message,
      backgroundColor,
      textColor: textColor,
      gravity: gravity,
      duration: duration,
    );
  }

  static void _showToast(
      BuildContext context,
      String message,
      Color backgroundColor, {
        Color textColor = Colors.white,
        ToastGravity gravity = ToastGravity.BOTTOM,
        Duration duration = const Duration(seconds: 2),
      }) {
    FToast fToast = FToast();
    fToast.init(context);
    Widget toast = Container(
      padding: const EdgeInsets.symmetric(horizontal: 24.0, vertical: 12.0),
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(25.0),
        color: backgroundColor,
      ),
      child: Text(
        message,
        style: TextStyle(color: textColor),
      ),
    );

    fToast.showToast(
      child: toast,
      toastDuration: duration,
      gravity: gravity,
    );
  }
}
