-- Insert Permission
INSERT INTO "Permissions" ("Name", "Description")
VALUES
    ('write.all', 'Create and update all modules'),
    ('write', 'Create only task'),
    ('read.all', 'Read all modules'),
    ('read', 'Read only Task'),
    ('update.all', 'Update all modules'),
    ('update', 'Update only Task'),
    ('delete.all', 'Delete all modules'),
    ('delete', 'Delete only Task');

-- Insert roles: admin y user
INSERT INTO "Role" ("Name", "Description")
VALUES
    ('admin', 'Administrator role with all permissions'),
    ('user', 'User role with limited permissions');

-- Insert RolePermission for user (no permissions with 'all')
INSERT INTO "RolePermission" ("RoleId", "PermissionId")
SELECT 2, "Id" FROM "Permissions" WHERE "Name" NOT LIKE '%all%';

-- Insert RolePermission -> Admin
INSERT INTO "RolePermission" ("RoleId", "PermissionId")
VALUES
    (1, (SELECT "Id" FROM "Permissions" WHERE "Name" = 'write.all')),
    (1, (SELECT "Id" FROM "Permissions" WHERE "Name" = 'read.all')),
    (1, (SELECT "Id" FROM "Permissions" WHERE "Name" = 'update.all')),
    (1, (SELECT "Id" FROM "Permissions" WHERE "Name" = 'delete.all'));

-- Insert RolePermission -> User
INSERT INTO "RolePermission" ("RoleId", "PermissionId")
VALUES
    (2, (SELECT "Id" FROM "Permissions" WHERE "Name" = 'write')),
    (2, (SELECT "Id" FROM "Permissions" WHERE "Name" = 'read')),
    (2, (SELECT "Id" FROM "Permissions" WHERE "Name" = 'update')),
    (2, (SELECT "Id" FROM "Permissions" WHERE "Name" = 'delete'));

