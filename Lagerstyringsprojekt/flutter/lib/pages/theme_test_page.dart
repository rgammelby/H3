import 'package:flutter/material.dart';

class ThemeTestPage extends StatefulWidget {
  const ThemeTestPage({super.key});

  @override
  _ThemeTestPageState createState() => _ThemeTestPageState();
}

class _ThemeTestPageState extends State<ThemeTestPage> {
  bool useOrangeTheme = false;

  @override
  Widget build(BuildContext context) {
    return Theme(
      data: useOrangeTheme
          ? ThemeData(
        brightness: Brightness.light,
        primaryColor: Colors.orange,
        colorScheme: ColorScheme.light(primary: Colors.orange),
        scaffoldBackgroundColor: Colors.orange.shade100,
        textTheme: TextTheme(
          bodyLarge: TextStyle(color: Colors.black),
        ),
      )
          : Theme.of(context), // Fallback to the app's default theme
      child: Scaffold(
        appBar: AppBar(
          title: Text('Theme Test Page'),
        ),
        body: Center(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Text(
                'This is the Theme Test Page!',
                style: Theme.of(context).textTheme.bodyLarge,
              ),
              SizedBox(height: 20),
              ElevatedButton(
                onPressed: () {
                  setState(() {
                    useOrangeTheme = !useOrangeTheme;
                  });
                },
                child: Text('Toggle Orange Theme'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
