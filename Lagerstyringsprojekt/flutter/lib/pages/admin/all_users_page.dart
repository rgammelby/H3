import 'package:flutter/material.dart';
import '../../services/api/user_service.dart';
import '../../services/global/localization.dart';

class AllUsersPage extends StatefulWidget {
  const AllUsersPage({super.key});

  @override
  _AllUsersPageState createState() => _AllUsersPageState();
}

class _AllUsersPageState extends State<AllUsersPage> {
  late Future<List<dynamic>> _usersFuture;
  List<dynamic> _allUsers = [];
  List<dynamic> _filteredUsers = [];
  final TextEditingController _searchController = TextEditingController();

  @override
  void initState() {
    super.initState();
    _usersFuture = UserService.getAllUsers();
    _fetchUsers();
    _searchController.addListener(_onSearchChanged);
  }

  Future<void> _fetchUsers() async {
    try {
      final users = await UserService.getAllUsers();
      setState(() {
        _allUsers = users;
        _filteredUsers = List.from(users);
      });
    } catch (e) {
      print('Error fetching users: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(AppLocalizations.of(context).translate('error_fetching_users')),
        ),
      );
    }
  }

  void _onSearchChanged() {
    final query = _searchController.text.toLowerCase();
    setState(() {
      _filteredUsers = _allUsers.where((user) {
        final userName =
        '${user['firstName'] ?? ''} ${user['lastName'] ?? ''}'.toLowerCase();
        return userName.contains(query);
      }).toList();
    });
  }

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  void _showDeleteDialog(int userID) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text(AppLocalizations.of(context).translate('delete_user_title')),
          content: Text(AppLocalizations.of(context).translate('delete_user_message')),
          actions: [
            TextButton(
              onPressed: () async {
                Navigator.pop(context);
                await _softDeleteUser(userID);
              },
              child: Text(AppLocalizations.of(context).translate('soft_delete')),
            ),
            TextButton(
              onPressed: () async {
                Navigator.pop(context);
                await _hardDeleteUser(userID);
              },
              child: Text(AppLocalizations.of(context).translate('hard_delete')),
            ),
            TextButton(
              onPressed: () {
                Navigator.pop(context);
              },
              child: Text(AppLocalizations.of(context).translate('cancel')),
            ),
          ],
        );
      },
    );
  }

  Future<void> _softDeleteUser(int userID) async {
    try {
      final result = await UserService.softDeleteUser(userID);
      if (result['success']) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(AppLocalizations.of(context).translate('user_soft_deleted')),
          ),
        );
        _fetchUsers();
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(AppLocalizations.of(context).translate('failed_soft_delete_user')),
          ),
        );
      }
    } catch (e) {
      print('Error during soft delete: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(AppLocalizations.of(context).translate('soft_delete_error')),
        ),
      );
    }
  }

  Future<void> _hardDeleteUser(int userID) async {
    try {
      final result = await UserService.hardDeleteUser(userID);
      if (result['success']) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(AppLocalizations.of(context).translate('user_hard_deleted')),
          ),
        );
        _fetchUsers();
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(AppLocalizations.of(context).translate('failed_hard_delete_user')),
          ),
        );
      }
    } catch (e) {
      print('Error during hard delete: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(AppLocalizations.of(context).translate('hard_delete_error')),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(AppLocalizations.of(context).translate('all_users')),
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: TextField(
              controller: _searchController,
              decoration: InputDecoration(
                hintText: AppLocalizations.of(context).translate('search_hint_users'),
                prefixIcon: const Icon(Icons.search),
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(8.0),
                ),
              ),
            ),
          ),
          Expanded(
            child: FutureBuilder<List<dynamic>>(
              future: _usersFuture,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.waiting) {
                  return const Center(child: CircularProgressIndicator());
                } else if (snapshot.hasError) {
                  return Center(
                    child: Text(AppLocalizations.of(context).translate('error_loading_users')),
                  );
                } else if (_filteredUsers.isEmpty) {
                  return Center(
                    child: Text(AppLocalizations.of(context).translate('no_users_found')),
                  );
                } else {
                  return ListView.builder(
                    itemCount: _filteredUsers.length,
                    itemBuilder: (context, index) {
                      final user = _filteredUsers[index];
                      return Card(
                        margin: const EdgeInsets.symmetric(
                            vertical: 8.0, horizontal: 16.0),
                        child: ListTile(
                          leading: user['profileImagePath'] != null
                              ? CircleAvatar(
                            backgroundImage:
                            NetworkImage(user['profileImagePath']),
                            radius: 20,
                          )
                              : const Icon(Icons.person, color: Colors.blueAccent),
                          title: Text(
                            '${user['firstName'] ?? AppLocalizations.of(context).translate('no_first_name')} ${user['lastName'] ?? ''}',
                          ),
                          subtitle: Text(
                            '${AppLocalizations.of(context).translate('email')}: ${user['email'] ?? AppLocalizations.of(context).translate('no_email')}\n'
                                '${AppLocalizations.of(context).translate('phone')}: ${user['phoneNum'] ?? AppLocalizations.of(context).translate('no_phone')}',
                          ),
                          trailing: IconButton(
                            icon: const Icon(Icons.delete, color: Colors.red),
                            onPressed: () =>
                                _showDeleteDialog(user['userID']),
                          ),
                          isThreeLine: true,
                        ),
                      );
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
