import 'package:flutter/material.dart';

class DebugButtons extends StatelessWidget {
  final VoidCallback onBypassAdmin;
  final VoidCallback onBypassUser;

  const DebugButtons({
    super.key,
    required this.onBypassAdmin,
    required this.onBypassUser,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Text(
          'Debugage for thy leisure:',
          style: const TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
        ),
        const SizedBox(height: 10),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            ElevatedButton(
              onPressed: onBypassAdmin,
              child: const Text('Bypass as Admin'),
            ),
            ElevatedButton(
              onPressed: onBypassUser,
              child: const Text('Bypass as User'),
            ),
          ],
        ),
      ],
    );
  }
}
