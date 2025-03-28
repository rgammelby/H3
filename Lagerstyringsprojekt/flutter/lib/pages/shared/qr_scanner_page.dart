import 'package:flutter/material.dart';
import '../../services/database/qr_service.dart';
import '../../widgets/qr_scanner_widget.dart';
import '../user/return_device_page.dart';
import '../../services/global/localization.dart';

class QRScannerPage extends StatefulWidget {
  final String role;
  final int userID;

  const QRScannerPage({super.key, required this.role, required this.userID});

  @override
  _QRScannerPageState createState() => _QRScannerPageState();
}

class _QRScannerPageState extends State<QRScannerPage> {
  String scanResult = ""; // Dynamic based on locale
  bool isProcessing = false;

  @override
  void initState() {
    super.initState();
    QRService.connectToDatabase();
    scanResult = AppLocalizations.of(context).translate('qr_scanner_initial_message'); // Initialize
  }

  // Handle QR code scanning
  void handleScan(String qrCode) async {
    if (isProcessing) return;

    setState(() {
      scanResult = qrCode;
      isProcessing = true; // Prevent multiple triggers
    });

    bool isValid = await QRService.validateQRCode(qrCode);

    if (isValid) {
      showToast(AppLocalizations.of(context).translate('qr_code_valid_message'));
      Navigator.push(
        context,
        MaterialPageRoute(
          builder: (context) => ReturnDevicePage(userID: widget.userID),
        ),
      );
    } else {
      showToast(AppLocalizations.of(context).translate('qr_code_invalid_message'));
    }

    setState(() {
      isProcessing = false; // Allow new scans
    });
  }

  // Show a toast message
  void showToast(String message) {
    final scaffold = ScaffoldMessenger.of(context);
    scaffold.showSnackBar(
      SnackBar(
        content: Text(message),
        duration: const Duration(seconds: 2),
      ),
    );
  }

  @override
  void dispose() {
    QRService.disconnectFromDatabase();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(AppLocalizations.of(context).translate('qr_authentication_title')),
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: Text(
              AppLocalizations.of(context).translate('qr_scanner_prompt'),
              textAlign: TextAlign.center,
              style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
          ),
          Expanded(
            child: QRScannerWidget(
              onScanComplete: handleScan,
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: Text(
              scanResult,
              textAlign: TextAlign.center,
              style: const TextStyle(fontSize: 16, color: Colors.black87),
            ),
          ),
          ElevatedButton(
            onPressed: () {
              setState(() {
                scanResult = AppLocalizations.of(context).translate('qr_scanner_initial_message'); // Reset message
              });
            },
            child: Text(AppLocalizations.of(context).translate('activate_qr_scanner_button')),
          ),
        ],
      ),
    );
  }
}
