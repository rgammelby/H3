import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:provider/provider.dart';
import 'pages/shared/login_page.dart';
import 'pages/shared/home_page.dart';
import 'services/preferences/preferences_service.dart';
import 'services/global/theme_manager.dart';
import 'services/global/shake_toggle_theme.dart';
import 'services/global/localization.dart';
import 'theme/theme_provider.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();

  // Enforce portrait orientation, OTHERWISE IT IS VERY SLOW...
  await SystemChrome.setPreferredOrientations([
    DeviceOrientation.portraitUp,
    DeviceOrientation.portraitDown,
  ]);

  // Configure HTTP overrides for certificates
  HttpOverrides.global = MyHttpOverrides();

  // Initialize PreferencesService and load preferences
  final preferencesService = PreferencesService();
  await preferencesService.loadPreferences();

  // Initialize ThemeProvider with loaded preferences
  final themeProvider = ThemeProvider();
  themeProvider.setPrimaryThemeIndex(preferencesService.themeA);
  themeProvider.setSecondaryThemeIndex(preferencesService.themeB);
  themeProvider.applyTheme(themeProvider.themeA);

  // Initialize the app with MultiProvider
  runApp(
    MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (_) => themeProvider),
        Provider(
          create: (_) => ShakeToggleTheme(
            onShakeDetected: () {}, // Placeholder for shake actions
            isShakeEnabled: preferencesService.isShakeEnabled,
          ),
        ),
        Provider.value(value: preferencesService), // Provide PreferencesService globally
      ],
      child: MyApp(preferencesService: preferencesService),
    ),
  );
}

class MyApp extends StatefulWidget {
  final PreferencesService preferencesService;

  const MyApp({super.key, required this.preferencesService});

  static void setLocale(BuildContext context, Locale newLocale) {
    final _MyAppState? state = context.findAncestorStateOfType<_MyAppState>();
    state?.changeLocale(newLocale);
  }

  @override
  _MyAppState createState() => _MyAppState();
}

class _MyAppState extends State<MyApp> {
  late Locale _locale; // Initialize dynamically based on preferences

  @override
  void initState() {
    super.initState();
    _locale = Locale(widget.preferencesService.selectedLanguage); // Set the initial locale
  }

  void changeLocale(Locale locale) {
    setState(() {
      _locale = locale; // Dynamically update the locale
    });
  }

  @override
  Widget build(BuildContext context) {
    final shakeService = Provider.of<ShakeToggleTheme>(context, listen: false);
    ThemeManager.linkShakeToTheme(context, shakeService);

    return Consumer<ThemeProvider>(
      builder: (context, themeProvider, child) {
        return MaterialApp(
          title: 'Device Management App',
          theme: themeProvider.currentTheme,
          darkTheme: themeProvider.themeB,
          themeMode: ThemeMode.light,
          locale: _locale, // Use the dynamic locale
          supportedLocales: const [
            Locale('en'), // English
            Locale('ja'), // Japanese
            Locale('da'), // Danish
          ],
          localizationsDelegates: const [
            GlobalMaterialLocalizations.delegate,
            GlobalWidgetsLocalizations.delegate,
            GlobalCupertinoLocalizations.delegate,
            AppLocalizations.delegate,
          ],
          localeResolutionCallback: (locale, supportedLocales) {
            // Resolve locale or default to the first supported locale
            if (locale != null) {
              for (var supportedLocale in supportedLocales) {
                if (supportedLocale.languageCode == locale.languageCode) {
                  return supportedLocale;
                }
              }
            }
            return supportedLocales.first;
          },
          initialRoute: '/login',
          routes: {
            '/login': (context) => LoginPage(preferencesService: widget.preferencesService),
            '/home': (context) => HomePage(
              role: 'User',
              userID: 0,
              isDebugMode: true,
              preferencesService: widget.preferencesService, // Pass PreferencesService
            ),
          },
          debugShowCheckedModeBanner: false,
        );
      },
    );
  }
}

class MyHttpOverrides extends HttpOverrides {
  @override
  HttpClient createHttpClient(SecurityContext? context) {
    return super.createHttpClient(context)
      ..badCertificateCallback = (X509Certificate cert, String host, int port) => true;
  }
}
