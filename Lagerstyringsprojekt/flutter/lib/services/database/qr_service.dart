import 'package:sql_conn/sql_conn.dart';

class QRService {
  // Connect to the database
  static Future<void> connectToDatabase() async {
    try {
      await SqlConn.connect(
        ip: "XXX.XXX.XX.XX",
        port: "1433",
        databaseName: "db_AuthQR",
        username: "flutter_user",
        password: "password123",
        timeout: 30,
      );
      print("[${DateTime.now()}] Connected to SQL Server!");
    } catch (e) {
      print("[${DateTime.now()}] Database connection failed: $e");
    }
  }

  // Validate the scanned QR code
  static Future<bool> validateQRCode(String qrCode) async {
    try {
      String query =
          "SELECT * FROM AuthQR WHERE AuthCode = '$qrCode' AND IsValid = 1";
      var response = await SqlConn.readData(query);

      return response != "[]";
    } catch (e) {
      print("Error while validating QR Code: $e");
      return false;
    }
  }

  // Disconnect from the database
  static Future<void> disconnectFromDatabase() async {
    await SqlConn.disconnect();
  }
}
