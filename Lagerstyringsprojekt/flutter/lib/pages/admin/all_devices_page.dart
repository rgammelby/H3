import 'package:flutter/material.dart';
import '../../services/api/device_service.dart';
import 'edit_device_page.dart';
import 'device_restoration_page.dart';
import '../../services/global/localization.dart';

class AllDevicesPage extends StatefulWidget {
  const AllDevicesPage({super.key});

  @override
  _AllDevicesPageState createState() => _AllDevicesPageState();
}

class _AllDevicesPageState extends State<AllDevicesPage> {
  late Future<List<dynamic>> _devicesFuture;
  List<dynamic> _allDevices = [];
  List<dynamic> _filteredDevices = [];
  final TextEditingController _searchController = TextEditingController();

  @override
  void initState() {
    super.initState();
    _devicesFuture = DeviceService.getAllDevices();
    _fetchDevices();
    _searchController.addListener(_onSearchChanged);
  }

  @override
  void dispose() {
    _searchController.removeListener(_onSearchChanged);
    _searchController.dispose();
    super.dispose();
  }

  Future<void> _fetchDevices() async {
    try {
      final devices = await DeviceService.getAllDevices();
      setState(() {
        _allDevices = devices;
        _filteredDevices = List.from(devices);
      });
    } catch (e) {
      print('Error fetching devices: $e');
      _showSnackbar(AppLocalizations.of(context).translate('error_loading_devices'));
      setState(() {
        _filteredDevices = [];
      });
    }
  }

  void _onSearchChanged() {
    final query = _searchController.text.trim().toLowerCase();
    setState(() {
      _filteredDevices = _allDevices.where((device) {
        final deviceName = device['deviceName']?.toLowerCase() ?? '';
        final deviceType = device['deviceType']?.toLowerCase() ?? '';
        return deviceName.contains(query) || deviceType.contains(query);
      }).toList();
    });
  }

  void _showSnackbar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(content: Text(message), duration: const Duration(seconds: 3)),
    );
  }

  void _navigateToDeviceRestorationPage() {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => const DeviceRestorationPage(),
      ),
    ).then((_) => _fetchDevices()); // Refresh the device list
  }

  void _showDeleteDialog(int deviceID) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: Text(AppLocalizations.of(context).translate('delete_device_title')),
        content: Text(AppLocalizations.of(context).translate('delete_device_message')),
        actions: [
          TextButton(
            onPressed: () async {
              Navigator.pop(context);
              await _deleteDevice(deviceID, softDelete: true);
            },
            child: Text(AppLocalizations.of(context).translate('soft_delete')),
          ),
          TextButton(
            onPressed: () async {
              Navigator.pop(context);
              await _deleteDevice(deviceID, softDelete: false);
            },
            child: Text(AppLocalizations.of(context).translate('hard_delete')),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(AppLocalizations.of(context).translate('cancel')),
          ),
        ],
      ),
    );
  }

  Future<void> _deleteDevice(int deviceID, {required bool softDelete}) async {
    try {
      final result = softDelete
          ? await DeviceService.softDeleteDevice(deviceID)
          : await DeviceService.hardDeleteDevice(deviceID);

      if (result['success'] == true) {
        _showSnackbar(softDelete
            ? AppLocalizations.of(context).translate('device_soft_deleted')
            : AppLocalizations.of(context).translate('device_hard_deleted'));
        _fetchDevices();
      } else {
        _showSnackbar(AppLocalizations.of(context).translate('device_delete_failed'));
      }
    } catch (e) {
      print('Error deleting device: $e');
      _showSnackbar(AppLocalizations.of(context).translate('device_delete_error'));
    }
  }

  void _navigateToEditDevice(dynamic device) {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => EditDevicePage(device: device),
      ),
    ).then((value) {
      if (value == true) _fetchDevices(); // Refresh the list after editing
    });
  }

  Widget _buildDeviceCard(dynamic device) {
    return Card(
      margin: const EdgeInsets.symmetric(vertical: 8.0, horizontal: 16.0),
      elevation: 4,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(12.0),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Device Image
            if (device['imagePath'] != null && device['imagePath']!.isNotEmpty)
              Image.network(
                device['imagePath']!,
                width: 50,
                height: 50,
                fit: BoxFit.cover,
                loadingBuilder: (context, child, progress) {
                  if (progress == null) return child;
                  return const Center(child: CircularProgressIndicator());
                },
                errorBuilder: (context, error, stackTrace) =>
                const Icon(Icons.error, color: Colors.red, size: 50),
              )
            else
              const Icon(Icons.devices, color: Colors.grey, size: 50), // Fallback Icon

            const SizedBox(width: 16.0),

            // Device Details
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    device['deviceName'] ?? AppLocalizations.of(context).translate('unnamed_device'),
                    style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                  ),
                  Text(
                    "${AppLocalizations.of(context).translate('type')}: ${device['deviceType'] ?? AppLocalizations.of(context).translate('unknown_type')}",
                    style: const TextStyle(fontSize: 14, color: Colors.grey),
                  ),
                  Text("${AppLocalizations.of(context).translate('location')}: ${device['location'] ?? AppLocalizations.of(context).translate('no_location')}"),
                  Text("${AppLocalizations.of(context).translate('cupboard')}: ${device['cupboard'] ?? AppLocalizations.of(context).translate('no_cupboard')}"),
                  Text("${AppLocalizations.of(context).translate('quantity')}: ${device['quantity'] ?? 0}"),
                  const SizedBox(height: 8.0),
                  Text(
                    device['status'] ?? AppLocalizations.of(context).translate('unknown_status'),
                    style: TextStyle(
                      color: device['status'] == 'Available' ? Colors.green : Colors.red,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ],
              ),
            ),

            // Action Buttons
            Column(
              children: [
                TextButton.icon(
                  icon: const Icon(Icons.edit, color: Colors.blue),
                  label: Text(AppLocalizations.of(context).translate('edit')),
                  onPressed: () => _navigateToEditDevice(device),
                ),
                IconButton(
                  icon: const Icon(Icons.delete, color: Colors.red),
                  onPressed: () => _showDeleteDialog(device['deviceID']),
                ),
              ],
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
        title: Text(AppLocalizations.of(context).translate('all_devices')),
    ),
      body: Column(
        children: [
          // Search Bar
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: TextField(
              controller: _searchController,
              decoration: InputDecoration(
                hintText: AppLocalizations.of(context).translate('search_hint'),
                prefixIcon: const Icon(Icons.search),
                border: OutlineInputBorder(borderRadius: BorderRadius.circular(8.0)),
              ),
            ),
          ),
          // Device Restoration Button
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: SizedBox(
              width: double.infinity,
              child: ElevatedButton.icon(
                onPressed: _navigateToDeviceRestorationPage,
                icon: const Icon(Icons.restore, color: Colors.white),
                label: Text(AppLocalizations.of(context).translate('device_restoration')),
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.green,
                  shape: BeveledRectangleBorder(),
                  padding: const EdgeInsets.symmetric(vertical: 16.0),
                ),
              ),
            ),
          ),
          // Device List
          Expanded(
            child: FutureBuilder<List<dynamic>>(
              future: _devicesFuture,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.waiting) {
                  return const Center(child: CircularProgressIndicator());
                } else if (snapshot.hasError) {
                  return Center(
                    child: Text(
                      AppLocalizations.of(context).translate('loading_error'),
                    ),
                  );
                } else if (_filteredDevices.isEmpty) {
                  return Center(
                    child: Text(
                      AppLocalizations.of(context).translate('no_devices_found'),
                    ),
                  );
                } else {
                  return ListView.builder(
                    itemCount: _filteredDevices.length,
                    itemBuilder: (context, index) {
                      return _buildDeviceCard(_filteredDevices[index]);
                    },
                  );
                }
              },
            ),
          ),
        ],
      ),
    );
  }
}
