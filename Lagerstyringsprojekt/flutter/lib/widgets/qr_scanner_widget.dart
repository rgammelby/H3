import 'package:flutter/material.dart';
import 'package:mobile_scanner/mobile_scanner.dart';

class QRScannerWidget extends StatelessWidget {
  final void Function(String qrCode) onScanComplete;

  const QRScannerWidget({super.key, required this.onScanComplete});

  @override
  Widget build(BuildContext context) {
    MobileScannerController scannerController = MobileScannerController();

    return MobileScanner(
      controller: scannerController,
      onDetect: (BarcodeCapture capture) {
        final List<Barcode> barcodes = capture.barcodes;
        for (final barcode in barcodes) {
          final String? code = barcode.rawValue;
          if (code != null) {
            onScanComplete(code); // Notify the parent about the scanned code
            scannerController.stop(); // Stop the scanner
            break;
          }
        }
      },
    );
  }
}
