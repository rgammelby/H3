import 'dart:io';
import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../../services/api/device_service.dart';
import '../../widgets/image_upload_widget.dart';
import '../../services/global/localization.dart';

class CreateDevicesPage extends StatefulWidget {
  const CreateDevicesPage({super.key});

  @override
  _CreateDevicesPageState createState() => _CreateDevicesPageState();
}

class _CreateDevicesPageState extends State<CreateDevicesPage> {
  final TextEditingController _deviceNameController = TextEditingController();
  final TextEditingController _deviceTypeController = TextEditingController();
  final TextEditingController _locationController = TextEditingController();
  final TextEditingController _cupboardController = TextEditingController();
  final TextEditingController _quantityController = TextEditingController();

  String _status = "Available"; // Default device status
  bool _isLoading = false; // Show loading spinner during API calls
  File? _deviceImage; // Store uploaded device image

  // Validate input fields
  bool _validateInputs() {
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
      _showSnackbar(AppLocalizations.of(context).translate('quantity_limit_message'));
      return false;
    }

    return true;
  }

  Future<void> _createDevice() async {
    if (!_validateInputs()) return;

    setState(() {
      _isLoading = true;
    });

    try {
      final deviceData = {
        "DeviceName": _deviceNameController.text.trim(),
        "DeviceType": _deviceTypeController.text.trim(),
        "Location": _locationController.text.trim(),
        "Cupboard": _cupboardController.text.trim(),
        "Quantity": int.parse(_quantityController.text.trim()),
        "Status": _status,
      };

      final result = await DeviceService.createDevice(deviceData);

      if (result['success'] == true && _deviceImage != null && result.containsKey('deviceID')) {
        final imageUploadResult = await DeviceService.uploadDeviceImage(
          result['deviceID'],
          base64Encode(_deviceImage!.readAsBytesSync()),
          _deviceImage!.path.split('/').last,
        );

        if (!imageUploadResult['success']) {
          _showSnackbar(AppLocalizations.of(context).translate('image_upload_failed'));
        }
      }

      if (result['success'] == true) {
        _clearForm();
        _showSnackbar(AppLocalizations.of(context).translate('device_created_success'));
      } else {
        _showSnackbar(result['message'] ?? AppLocalizations.of(context).translate('device_creation_failed'));
      }
    } catch (e) {
      _showSnackbar('${AppLocalizations.of(context).translate('error_message')}: ${e.toString()}');
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  void _clearForm() {
    _deviceNameController.clear();
    _deviceTypeController.clear();
    _locationController.clear();
    _cupboardController.clear();
    _quantityController.clear();
    setState(() {
      _status = "Available";
      _deviceImage = null;
    });
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
        title: Text(localizations.translate('create_device_title')),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: SingleChildScrollView(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              TextField(
                controller: _deviceNameController,
                decoration: InputDecoration(
                  labelText: localizations.translate('device_name'),
                ),
              ),
              const SizedBox(height: 10),
              TextField(
                controller: _deviceTypeController,
                decoration: InputDecoration(
                  labelText: localizations.translate('device_type'),
                ),
              ),
              const SizedBox(height: 10),
              TextField(
                controller: _locationController,
                decoration: InputDecoration(
                  labelText: localizations.translate('location'),
                ),
              ),
              const SizedBox(height: 10),
              TextField(
                controller: _cupboardController,
                decoration: InputDecoration(
                  labelText: localizations.translate('cupboard'),
                ),
              ),
              const SizedBox(height: 10),
              TextField(
                controller: _quantityController,
                decoration: InputDecoration(
                  labelText: localizations.translate('quantity'),
                ),
                keyboardType: TextInputType.number, // Show numeric keyboard
                inputFormatters: [
                  FilteringTextInputFormatter.digitsOnly, // Allow digits only
                  LengthLimitingTextInputFormatter(5), // Limit input to 5 digits
                ],
              ),
              const SizedBox(height: 20),
              DropdownButtonFormField<String>(
                value: _status,
                decoration: InputDecoration(labelText: localizations.translate('device_status')),
                onChanged: (String? newValue) {
                  setState(() {
                    _status = newValue!;
                  });
                },
                items: <String>[
                  'Available',
                  'Unavailable',
                  'Removed',
                ].map<DropdownMenuItem<String>>((String value) {
                  return DropdownMenuItem<String>(
                    value: value,
                    child: Text(localizations.translate('status_${value.toLowerCase()}')),
                  );
                }).toList(),
              ),
              const SizedBox(height: 20),
              ImageUploadWidget(
                onImageSelected: (File image) {
                  setState(() {
                    _deviceImage = image;
                  });
                },
              ),
              const SizedBox(height: 20),
              _isLoading
                  ? const CircularProgressIndicator()
                  : ElevatedButton(
                onPressed: _createDevice,
                style: ElevatedButton.styleFrom(
                  minimumSize: const Size(double.infinity, 50),
                ),
                child: Text(localizations.translate('create_device_button')),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
