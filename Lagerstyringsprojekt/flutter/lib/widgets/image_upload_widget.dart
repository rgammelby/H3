import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

class ImageUploadWidget extends StatefulWidget {
  final Function(File) onImageSelected;

  const ImageUploadWidget({super.key, required this.onImageSelected});

  @override
  _ImageUploadWidgetState createState() => _ImageUploadWidgetState();
}

class _ImageUploadWidgetState extends State<ImageUploadWidget> {
  static const _channel =
  MethodChannel('com.example.bestworstapp/image_picker');
  File? _selectedImage;

  Future<void> _pickImage() async {
    try {
      if (Platform.isAndroid) {
        // Call native Android method
        final String? imagePath = await _channel.invokeMethod('pickImage');
        if (imagePath != null) {
          setState(() {
            _selectedImage = File(imagePath);
          });
          widget.onImageSelected(_selectedImage!); // Notify parent widget
        }
      } else if (Platform.isWindows) {
        // Placeholder for Windows (YAAAAAAAAASSSSSSSS, but no) implementation
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Image picking on Windows not supported yet.')),
        );
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Unsupported platform.')),
        );
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error picking image: $e')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        GestureDetector(
          onTap: _pickImage,
          child: Container(
            height: 150, // Fixed height otherwise it breaks
            width: 150,  // Fixed width OTHERWISE IT BREAKS
            decoration: BoxDecoration(
              border: Border.all(color: Colors.grey),
              borderRadius: BorderRadius.circular(8),
            ),
            child: _selectedImage != null
                ? ClipRRect(
              borderRadius: BorderRadius.circular(8),
              child: Image.file(_selectedImage!, fit: BoxFit.cover),
            )
                : const Center(child: Text('Select Image')),
          ),
        ),
        const SizedBox(height: 10),
        if (_selectedImage != null)
          ElevatedButton(
            onPressed: () {
              setState(() {
                _selectedImage = null; // Clear the image
              });
            },
            child: const Text('Remove Image'),
          ),
      ],
    );
  }
}
