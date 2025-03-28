import 'package:flutter/material.dart';
import '../pages/admin/all_users_page.dart';
import '../pages/admin/all_devices_page.dart';
import '../pages/admin/create_devices_page.dart';
import '../pages/admin/create_users_page.dart';
import '../pages/user/borrow_device_page.dart';
import '../pages/shared/qr_scanner_page.dart';
import '../pages/user/my_devices_page.dart';
import '../pages/shared/login_page.dart';
import '../pages/shared/home_page.dart';
import '../pages/profile/profile_page.dart';
import '../pages/settings/settings_page.dart';
import '../../services/preferences/preferences_service.dart';
import '../../services/global/localization.dart';

class MenuDrawer extends StatelessWidget {
  final PreferencesService preferencesService;
  final String role;
  final int userID;

  const MenuDrawer({
    super.key,
    required this.preferencesService,
    required this.role,
    required this.userID,
  });

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context); // Get the current theme
    final colorScheme = theme.colorScheme;

    // Access localized strings
    final localizations = AppLocalizations.of(context);

    return Drawer(
      child: Column(
        children: [
          DrawerHeader(
            decoration: BoxDecoration(
              color: colorScheme.primary,
            ),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                Text(
                  localizations.translate('menu_title'),
                  style: TextStyle(
                    color: colorScheme.onPrimary,
                    fontSize: 24,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 10),
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    OutlinedButton.icon(
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) => ProfilePage(userID: userID),
                          ),
                        );
                      },
                      icon: Icon(Icons.person, color: colorScheme.onPrimary),
                      label: Text(
                        localizations.translate('profile'),
                        style: TextStyle(color: colorScheme.onPrimary),
                      ),
                    ),
                    OutlinedButton.icon(
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) => SettingsPage(
                              preferencesService: preferencesService,
                              userID: userID,
                            ),
                          ),
                        );
                      },
                      icon: Icon(Icons.settings, color: colorScheme.onPrimary),
                      label: Text(
                        localizations.translate('settings'),
                        style: TextStyle(color: colorScheme.onPrimary),
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),
          // Main menu items
          Expanded(
            child: ListView(
              padding: EdgeInsets.zero,
              children: [
                ..._generateRoleBasedItems(context, localizations, colorScheme),
                const Divider(),
                ListTile(
                  leading: Icon(Icons.logout, color: colorScheme.error),
                  title: Text(
                    localizations.translate('logout'),
                    style: TextStyle(color: colorScheme.error),
                  ),
                  onTap: () {
                    Navigator.pushReplacement(
                      context,
                      MaterialPageRoute(
                        builder: (context) => LoginPage(
                          preferencesService: preferencesService,
                        ),
                      ),
                    );
                  },
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  List<Widget> _generateRoleBasedItems(
      BuildContext context, AppLocalizations localizations, ColorScheme colorScheme) {
    List<Widget> items = [
      ListTile(
        leading: Icon(Icons.home, color: colorScheme.onSurface),
        title: Text(localizations.translate('home'),
            style: TextStyle(color: colorScheme.onSurface)),
        onTap: () {
          Navigator.pushReplacement(
            context,
            MaterialPageRoute(
              builder: (context) => HomePage(
                role: role,
                userID: userID,
                preferencesService: preferencesService,
                isDebugMode: false,
              ),
            ),
          );
        },
      ),
    ];

    if (role == 'Admin') {
      items.addAll([
        ListTile(
          leading: Icon(Icons.people, color: colorScheme.onSurface),
          title: Text(localizations.translate('all_users'),
              style: TextStyle(color: colorScheme.onSurface)),
          onTap: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => const AllUsersPage()),
            );
          },
        ),
        ListTile(
          leading: Icon(Icons.devices, color: colorScheme.onSurface),
          title: Text(localizations.translate('all_devices'),
              style: TextStyle(color: colorScheme.onSurface)),
          onTap: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => const AllDevicesPage()),
            );
          },
        ),
        ListTile(
          leading: Icon(Icons.add, color: colorScheme.onSurface),
          title: Text(localizations.translate('create_device'),
              style: TextStyle(color: colorScheme.onSurface)),
          onTap: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => const CreateDevicesPage()),
            );
          },
        ),
        ListTile(
          leading: Icon(Icons.person_add, color: colorScheme.onSurface),
          title: Text(localizations.translate('create_users'),
              style: TextStyle(color: colorScheme.onSurface)),
          onTap: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => const CreateUsersPage()),
            );
          },
        ),
      ]);
    }

    if (role == 'User') {
      items.add(
        ListTile(
          leading: Icon(Icons.perm_device_information, color: colorScheme.onSurface),
          title: Text(localizations.translate('my_devices'),
              style: TextStyle(color: colorScheme.onSurface)),
          onTap: () {
            Navigator.push(
              context,
              MaterialPageRoute(
                builder: (context) => MyDevicesPage(userID: userID),
              ),
            );
          },
        ),
      );
    }

    items.addAll([
      ListTile(
        leading: Icon(Icons.shopping_bag, color: colorScheme.onSurface),
        title: Text(localizations.translate('borrow_device'),
            style: TextStyle(color: colorScheme.onSurface)),
        onTap: () {
          Navigator.push(
            context,
            MaterialPageRoute(
              builder: (context) => BorrowDevicePage(userID: userID),
            ),
          );
        },
      ),
      ListTile(
        leading: Icon(Icons.keyboard_return, color: colorScheme.onSurface),
        title: Text(localizations.translate('return_device'),
            style: TextStyle(color: colorScheme.onSurface)),
        onTap: () {
          Navigator.push(
            context,
            MaterialPageRoute(
              builder: (context) => QRScannerPage(role: role, userID: userID),
            ),
          );
        },
      ),
    ]);

    return items;
  }
}
