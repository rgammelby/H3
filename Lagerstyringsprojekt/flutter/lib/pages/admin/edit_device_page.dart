import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'dart:convert';
import '../../services/api/device_service.dart';
import '../../services/global/localization.dart';

class EditDevicePage extends StatefulWidget {
  final dynamic device;

  const EditDevicePage({required this.device, super.key});

  @override
  _EditDevicePageState createState() => _EditDevicePageState();
}

class _EditDevicePageState extends State<EditDevicePage> {
  final _deviceNameController = TextEditingController();
  final _deviceTypeController = TextEditingController();
  final _locationController = TextEditingController();
  final _cupboardController = TextEditingController();
  final _quantityController = TextEditingController();

  String _status = "Available"; // Default status
  bool _isLoading = false;

  @override
  void initState() {
    super.initState();
    _initializeFields();
  }

  void _initializeFields() {
    final device = widget.device;
    _deviceNameController.text = device['deviceName'] ?? '';
    _deviceTypeController.text = device['deviceType'] ?? '';
    _locationController.text = device['location'] ?? '';
    _cupboardController.text = device['cupboard'] ?? '';
    _quantityController.text = device['quantity']?.toString() ?? '0';
    _status = device['status'] ?? 'Available';
  }

  Future<void> _updateDevice() async {
    if (widget.device['deviceID'] == null) {
      _showSnackbar(AppLocalizations.of(context).translate('missing_device_id'));
      return;
    }
    if (!_validateFields()) return;

    setState(() => _isLoading = true);

    try {
      final updatedData = {
        'deviceID': widget.device['deviceID'],
        'deviceName': _deviceNameController.text.trim(),
        'deviceType': _deviceTypeController.text.trim(),
        'location': _locationController.text.trim(),
        'cupboard': _cupboardController.text.trim(),
        'quantity': int.tryParse(_quantityController.text.trim()) ?? 0,
        'status': _status,
        'isDeleted': widget.device['isDeleted'] ?? false,
        'deletedAt': widget.device['deletedAt'],
        'imagePath': widget.device['imagePath']
            ?.replaceAll('https://XXX.XXX.XX.XX:5048/', ''),
        'createdAt': widget.device['createdAt'] ??
            DateTime.now().toIso8601String(),
        'updatedAt': DateTime.now().toIso8601String(),
      };

      // Debugging: Log the payload being sent
      //print('PUT Payload: ${jsonEncode(updatedData)}');

      final result =
      await DeviceService.updateDevice(widget.device['deviceID'], updatedData);

      // Debugging: Log the API response
      //print('API Response: $result');

      if (result['success'] == true) {
        _showSnackbar(AppLocalizations.of(context).translate('device_updated_success'));
        Navigator.pop(context, true);
      } else {
        _showSnackbar(result['message'] ?? AppLocalizations.of(context).translate('device_update_failed'));
      }
    } catch (e) {
      print('Error during update: $e');
      _showSnackbar('${AppLocalizations.of(context).translate('error_message')}: ${e.toString()}');
    } finally {
      setState(() => _isLoading = false);
    }
  }

  bool _validateFields() {
    final fields = [
      _deviceNameController.text.trim(),
      _deviceTypeController.text.trim(),
      _locationController.text.trim(),
      _cupboardController.text.trim(),
      _quantityController.text.trim(),
    ];

    if (fields.contains('')) {
      _showSnackbar(AppLocalizations.of(context).translate('fill_required_fields'));
      return false;
    }

    final quantity = int.tryParse(_quantityController.text.trim());
    if (quantity == null || quantity < 0) {
      _showSnackbar(AppLocalizations.of(context).translate('valid_quantity_message'));
      return false;
    }

    if (quantity > 10000) {
      _showSnackbar(AppLocalizations.of(context).translate('quantity_limit_message')); // CANNOT EXCEED 10000
      return false;
    }

    return true;
  }

  void _showSnackbar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(content: Text(message), duration: const Duration(seconds: 3)),
    );
  }

  @override
  void dispose() {
    _deviceNameController.dispose();
    _deviceTypeController.dispose();
    _locationController.dispose();
    _cupboardController.dispose();
    _quantityController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final localizations = AppLocalizations.of(context);

    return Scaffold(
      appBar: AppBar(
        title: Text(localizations.translate('edit_device_title')),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: SingleChildScrollView(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              _buildTextField(_deviceNameController, localizations.translate('device_name')),
              const SizedBox(height: 16.0),
              _buildTextField(_deviceTypeController, localizations.translate('device_type')),
              const SizedBox(height: 16.0),
              _buildTextField(_locationController, localizations.translate('location')),
              const SizedBox(height: 16.0),
              _buildTextField(_cupboardController, localizations.translate('cupboard')),
              const SizedBox(height: 16.0),
              _buildTextField(
                _quantityController,
                localizations.translate('quantity'),
                keyboardType: TextInputType.number,
                inputFormatters: [
                  FilteringTextInputFormatter.digitsOnly, // Restrict to digits
                  LengthLimitingTextInputFormatter(5), // Limit input to 5 digits
                ],
              ),
              const SizedBox(height: 16.0),
              DropdownButtonFormField<String>(
                value: _status,
                decoration: InputDecoration(
                  labelText: localizations.translate('device_status'), // "Device Status"
                  border: const OutlineInputBorder(),
                ),
                onChanged: (String? newValue) {
                  setState(() => _status = newValue!);
                },
                items: ['Available', 'Unavailable', 'Removed']
                    .map((value) => DropdownMenuItem<String>(
                  value: value,
                  child: Text(localizations.translate('status_${value.toLowerCase()}')), // Ensure localized labels
                ))
                    .toList(),
              ),
              const SizedBox(height: 20.0),
              _isLoading
                  ? const Center(child: CircularProgressIndicator())
                  : ElevatedButton(
                onPressed: _updateDevice, // "Update Device"
                style: ElevatedButton.styleFrom(
                  minimumSize: const Size(double.infinity, 50), // Full-width button
                ),
                child: Text(localizations.translate('update_device_button')),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildTextField(
      TextEditingController controller,
      String label, {
        TextInputType keyboardType = TextInputType.text,
        List<TextInputFormatter>? inputFormatters,
      }) {
    return TextField(
      controller: controller,
      decoration: InputDecoration(
        labelText: label,
        border: const OutlineInputBorder(),
      ),
      keyboardType: keyboardType,
      inputFormatters: inputFormatters,
    );
  }
}
