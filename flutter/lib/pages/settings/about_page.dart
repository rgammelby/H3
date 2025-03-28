import 'package:flutter/material.dart';

class AboutPage extends StatelessWidget {
  const AboutPage({super.key});

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context); // Fetch current theme dynamically

    return Scaffold(
      appBar: AppBar(
        title: Text(
          'About This App',
          style: theme.textTheme.bodyLarge?.copyWith(
            fontWeight: FontWeight.bold,
          ),
        ),
        backgroundColor: theme.appBarTheme.backgroundColor,
      ),
      body: Padding(
        padding: const EdgeInsets.all(24.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [

            Column(
              children: [
                Text(
                  'Made by:',
                  style: theme.textTheme.bodyLarge?.copyWith(
                    fontSize: 20,
                    fontWeight: FontWeight.bold,
                    color: theme.colorScheme.primary,
                  ),
                  textAlign: TextAlign.center,
                ),
                const SizedBox(height: 16),
                Text(
                  'Lucas,\nSascha,\nTian\n&\nRune',
                  style: theme.textTheme.bodyMedium?.copyWith(
                    fontSize: 18,
                  ),
                  textAlign: TextAlign.center,
                ),
              ],
            ),

            // Spacer for better layout
            Divider(
              color: theme.colorScheme.secondary,
              thickness: 1.5,
              indent: 32,
              endIndent: 32,
            ),

            Column(
              children: [
                const SizedBox(height: 16),
                Text(
                  'Thank you for using our app!',
                  style: theme.textTheme.bodyLarge?.copyWith(
                    fontSize: 20,
                    fontWeight: FontWeight.w600,
                  ),
                  textAlign: TextAlign.center,
                ),
              ],
            ),

            // Icon for Visual Pop
            Icon(
              Icons.favorite,
              size: 40,
              color: theme.colorScheme.secondary,
            ),
          ],
        ),
      ),
    );
  }
}
