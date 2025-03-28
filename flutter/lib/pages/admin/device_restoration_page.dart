import 'package:flutter/material.dart';
import '../../services/api/device_service.dart';
import '../../services/global/localization.dart';

class DeviceRestorationPage extends StatefulWidget {
  const DeviceRestorationPage({super.key});

  @override
  _DeviceRestorationPageState createState() => _DeviceRestorationPageState();
}

class _DeviceRestorationPageState extends State<DeviceRestorationPage> {
  late Future<List<dynamic>> _deletedDevicesFuture;
  final List<dynamic> _deletedDevices = [];

  @override
  void initState() {
    super.initState();
    _deletedDevicesFuture = _fetchDeletedDevices();
  }

  Future<List<dynamic>> _fetchDeletedDevices() async {
    try {
      final devices = await DeviceService.getDeletedDevices();
      setState(() {
        _deletedDevices.clear();
        _deletedDevices.addAll(devices);
      });
      return devices;
    } catch (e) {
      print('Error fetching deleted devices: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(AppLocalizations.of(context).translate('failed_to_load_deleted_devices')), // Localized
        ),
      );
      return [];
    }
  }

  Future<void> _restoreDevice(int deviceID) async {
    try {
      final result = await DeviceService.restoreDevice(deviceID);
      if (result['success']) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(AppLocalizations.of(context).translate('device_restored_success')), // Localized
          ),
        );
        setState(() {
          _deletedDevices.removeWhere((device) => device['deviceID'] == deviceID);
        });
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(result['message'] ?? AppLocalizations.of(context).translate('failed_to_restore_device')), // Localized
          ),
        );
      }
    } catch (e) {
      print('Error during device restoration: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(AppLocalizations.of(context).translate('failed_to_restore_device')), // Localized
        ),
      );
    }
  }

  Widget _buildDeviceCard(dynamic device) {
    return Card(
      margin: const EdgeInsets.symmetric(vertical: 8.0, horizontal: 16.0),
      elevation: 6,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(15.0),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Row(
          children: [
            // Icon or Placeholder for Device
            CircleAvatar(
              backgroundColor: Colors.grey.shade200,
              radius: 30,
              child: Icon(
                Icons.devices,
                size: 30,
                color: Theme.of(context).primaryColor, // Adaptive icon color
              ),
            ),
            const SizedBox(width: 16.0),

            // Device Information
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    device['deviceName'] ??
                        AppLocalizations.of(context).translate('unnamed_device'), // Localized
                    style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 4.0),
                  Text(
                    "${AppLocalizations.of(context).translate('type')}: ${device['deviceType'] ?? AppLocalizations.of(context).translate('unknown_type')}", // Localized
                    style: Theme.of(context).textTheme.bodyMedium,
                  ),
                  const SizedBox(height: 4.0),
                  Text(
                    "${AppLocalizations.of(context).translate('location')}: ${device['location'] ?? AppLocalizations.of(context).translate('unknown_location')}", // Localized
                    style: Theme.of(context).textTheme.bodyMedium,
                  ),
                ],
              ),
            ),

            // Restore Button
            ElevatedButton.icon(
              onPressed: () => _restoreDevice(device['deviceID']),
              icon: const Icon(Icons.restore, size: 16, color: Colors.white),
              label: Text(
                AppLocalizations.of(context).translate('restore_button'), // Localized
                style: const TextStyle(fontSize: 14, color: Colors.white),
              ),
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.green,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12.0),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(AppLocalizations.of(context).translate('restore_deleted_devices_title')), // Localized
        centerTitle: true,
      ),
      body: FutureBuilder<List<dynamic>>(
        future: _deletedDevicesFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          } else if (snapshot.hasError) {
            return Center(
              child: Text(
                '${AppLocalizations.of(context).translate('error_fetching_devices')}:\n${snapshot.error}', // Localized
                textAlign: TextAlign.center,
                style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                  color: Colors.redAccent, // Error color
                ),
              ),
            );
          } else if (_deletedDevices.isEmpty) {
            return Center(
              child: Text(
                AppLocalizations.of(context).translate('no_deleted_devices'), // Localized
                style: Theme.of(context).textTheme.bodyMedium,
              ),
            );
          } else {
            return ListView.builder(
              itemCount: _deletedDevices.length,
              itemBuilder: (context, index) {
                return _buildDeviceCard(_deletedDevices[index]);
              },
            );
          }
        },
      ),
    );
  }
}
