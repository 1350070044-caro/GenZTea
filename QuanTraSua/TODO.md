# Admin Area Separation Task
Current: c:/Users/caro/Desktop/backup1 (1)/QuanTraSua/

## Steps (Will update progress ✅)

### Phase 1: Setup Structure
- ✅ 1. Update Program.cs - Add Areas route support
- ✅ 2. Create Areas/Admin/Controllers/AdminController.cs - Login + protected actions
- ✅ 3. Create Areas/Admin/Views/Admin/Login.cshtml - Login form
- ✅ 4. Create Areas/Admin/Views/Shared/_ViewImports.cshtml & _ViewStart.cshtml - Area view setup

### Phase 2: Migrate Dashboard
- ✅ 5. Create Areas/Admin/Views/Admin/Dashboard.cshtml - Copy from Home/AdminDashboard
- ✅ 6. Update HomeController.cs - Remove AdminDashboard action

### Phase 3: Migrate Product Management
- ✅ 7. Copy Product views/logic to Areas/Admin/Products/*
- ✅ 8. Update ProductController.cs - Add session auth check

### Phase 4: Migrate Order Management  
- ✅ 9. Create Areas/Admin/Views/Orders/Index.cshtml - Admin orders list
- ✅ 10. Update OrderController.cs - Protect Index, keep Create for users

### Phase 5: Testing & Cleanup
- ✅ 11. Add logout functionality (already in AdminController)
- [ ] 12. Test user flow (Menu → Order) & admin flow (Login → Dashboard/Products/Orders)
- [ ] 13. Redirect old /Home/AdminDashboard to /Admin/Login
- [ ] 14. Complete ✅

Commands to run after all:
dotnet build
dotnet run --urls=http://localhost:5126
