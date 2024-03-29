# CLK.AspNetCoreLab


## [ASP.NET Core] PathPermission 身分授權範例

本篇範例程式，展示如何使用自定義的 PathPermission，進行URL層級身分授權(沒權限的URL，跳至302 Access Denied)。

範例原碼：https://github.com/Clark159/CLK.AspNetCoreLab

範例專案：PathPermissionLab

測試步驟：
  1. 權限設定於 MockPathPermissionRepository.cs。 
  2. 執行PathPermissionLab專案
  3. 於Account.Login頁面點擊Login:Clark按鈕，以Clark身分登入。(Clark擁有：「/Home/*」授權)
  4. 於Home.Index頁面，可以看到按鈕：Home.Add、Home.Remove、Home.Update。
  5. 依次點選按鈕，允許進入各自的頁面：/Home/Add、/Home/Remove、/Home/Update。(URL層級身分授權)
  6. 於Home.Index頁面，點擊登出按鈕，回到Account.Login頁面。
  7. 於Account.Login頁面點擊Login:Jane按鈕，以Jane身分登入。(Jane只擁有：「/Home/Add」授權)
  8. 於Home.Index頁面，可以看到按鈕：Home.Add、Home.Remove、Home.Update。
  9. 點選Home.Add按鈕，允許進入頁面：/Home/Add。(URL層級身分授權)
  10. 點選Home.Remove按鈕，不允許進入/Home/Remove頁面，並跳轉至302 Access Denied。(URL層級身分授權)
  11. 點選Home.Update按鈕，不允許進入/Home/Update頁面，並跳轉至302 Access Denied。(URL層級身分授權)
  
  
## [ASP.NET Core] OperationPermission 身分授權範例

本篇範例程式，展示如何使用自定義的 OperationPermission，進行View層級身分授權(沒權限看不到按鈕)、進行Action層級身分授權(沒權限跳至302 Access Denied)。

範例原碼：https://github.com/Clark159/CLK.AspNetCoreLab

範例專案：OperationPermissionLab

測試步驟：
  1. 權限設定於 MockOperationPermissionRepository.cs。 
  2. 執行OperationPermissionLab專案
  3. 於Account.Login頁面點擊Login:Clark按鈕，以Clark身分登入。(Clark擁有：Home.Add、Home.Remove、Home.Update授權)
  4. 於Home.Index頁面，可以看到按鈕：Home.Add、Home.Remove、Home.Update。(View層級身分授權)
  5. 依次點選按鈕，允許進入各自的頁面：Home.Add、Home.Remove、Home.Update。(Action層級身分授權)
  6. 於Home.Index頁面，點擊登出按鈕，回到Account.Login頁面。
  7. 於Account.Login頁面點擊Login:Jane按鈕，以Jane身分登入。(Jane擁有：Home.Add授權)
  8. 於Home.Index頁面，可以看到按鈕：Home.Add。(View層級身分授權、Home.Update是特地留下來測試用)
  9. 點選Home.Add按鈕，允許進入Home.Add頁面。(Action層級身分授權)
  10. 點選Home.Update按鈕，不允許進入Home.Update頁面，並跳轉至302 Access Denied。(Action層級身分授權)

參考資料：
  
  1. [ASP.NET Core 认证与授权7动态授权-雨夜朦朧](https://www.cnblogs.com/RainingNight/tag/Authorization/)  
  
  
## [ASP.NET Core] OAuth驗證後進行註冊的身分驗證範例

本篇範例展示如何在ASP.NET Core裡，使用OAuth驗證後要求用戶進行註冊，註冊完成才允許登入的身分驗證機制。(已註冊過直接登入)

範例原碼：https://github.com/Clark159/CLK.AspNetCoreLab

範例專案：OAuthToRegisterAuthenticationLab

環境準備：

- Google OAuth
  1. 至Google建立專案，並申請OAuth2.0用戶端。(https://console.cloud.google.com/home)
  2. 於申請OAuth頁面的「已授權的重新導向URI」欄位，輸入https://localhost:44311/Account/Google-OAuth-SignIn
  3. 於申請OAuth頁面取得ClientId、ClientSecret。
  4. 將上一個步驟取得的ClientId、ClientSecret，填寫至Startup.cs的AddGoogle程式碼區塊內。

- Facebook OAuth
  1. 至Facebook建立應用程式，並申請Facebook登入。(https://developers.facebook.com/apps/)
  2. 於申請Facebook登入頁面的「有效的 OAuth 重新導向 URI」欄位，輸入https://localhost:44311/Account/Facebook-OAuth-SignIn
  3. 於設定\基本資料頁面取得ClientId、ClientSecret。
  4. 將上一個步驟取得的ClientId、ClientSecret，填寫至Startup.cs的AddFacebook程式碼區塊內。
  
測試步驟：

- OAuth登入後進行註冊，狀態：用戶資料未註冊
  1. 於localhost網站的Account.Login頁面點擊LoginByGoogle按鈕，執行Account.ExternalLogin，並開啟Google網站進行OAuth的登入及授權。
  2. (背景作業)Google完成OAuth的登入及授權後，重新導向至https://localhost:44311/Account/Google-OAuth-SignIn。
  3. (背景作業)localhost網站將Google回傳的OAuth授權資料，儲存至ExternalCookie，並導頁至Account.ExternalSignIn。
  4. (背景作業)localhost網站執行Account.ExternalSignIn，使用ExternalCookie的OAuth授權資料，確認用戶沒有註冊資訊後，導頁至Account.Register。
  5. 於localhost網站的Account.Register頁面，填寫註冊資料後點擊Register按鈕完成用戶註冊後，將用戶資訊儲存至ApplicationCookie並轉頁至Home.Index頁面。(已登入)
  6. 於localhost網站的Home.Index頁面，可以看到目前登入的用戶資訊。：已登入，使用Google-OAuth。
  7. 於localhost網站的Home.Index頁面點擊Logout按鈕，進行登出後完成測試。

- OAuth登入後無須註冊，狀態：用戶資料已註冊
  1. 於localhost網站的Account.Login頁面點擊LoginByGoogle按鈕，執行Account.ExternalLogin，並開啟Google網站進行OAuth的登入及授權。
  2. (背景作業)Google完成OAuth的登入及授權後，重新導向至https://localhost:44311/Account/Google-OAuth-SignIn。
  3. (背景作業)localhost網站將Google回傳的OAuth授權資料，儲存至ExternalCookie，並導頁至Account.ExternalSignIn。
  4. (背景作業)localhost網站執行Account.ExternalSignIn，使用ExternalCookie的OAuth授權資料，確認用戶擁有註冊資訊後，將用戶資訊儲存至ApplicationCookie並轉頁至Home.Index頁面。(已登入)
  5. 於localhost網站的Home.Index頁面，可以看到目前登入的用戶資訊：已登入，使用Google-OAuth。
  6. 於localhost網站的Home.Index頁面點擊Logout按鈕，進行登出後完成測試。
  
- Password登入後無須註冊，狀態：用戶資料已註冊
  1. 於localhost網站的Account.Login頁面點擊LoginByPassword按鈕，執行Account.PasswordSignIn。
  2. (背景作業)localhost網站執行Account.PasswordSignIn，確認用戶密碼(Hash)後，將用戶資訊儲存至ApplicationCookie並轉頁至Home.Index頁面。(已登入)
  3. 於localhost網站的Home.Index頁面，可以看到目前登入的用戶資訊：已登入，使用Password。
  4. 於localhost網站的Home.Index頁面點擊Logout按鈕，進行登出後完成測試。
  
  
## [ASP.NET Core] Cookie/JwtBearer並存的身分驗證範例

本篇範例展示如何在ASP.NET Core裡，同時使用Cookie及JwtBearer的身分驗證機制。

範例原碼：https://github.com/Clark159/CLK.AspNetCoreLab

範例專案：CookieOrJwtBearerAuthenticationLab

測試步驟：

- Account.Login、狀態：未登入 
  1. 點擊GetTokenByPassword按鈕，將會使用Username、Password進行身分驗證，並發放Token回傳。
  2. 點擊GetUserByToken按鈕，將會使用步驟1取得的Token進行身分驗證，並回傳目前登入的User資料。(authenticationType="JwtBearer")
  3. 點擊Login按鈕，將會使用Username、Password進行身分驗證，並發放Cookie後轉頁至Home頁面。
  
- Home.Index、狀態：已登入 
  1. 進入頁面後，會先顯示目前登入的User資料。(authenticationType="Cookies")
  2. 點擊GetTokenByCookie按鈕，將會使用Cookie進行身分驗證，並發放Token回傳。
  3. 點擊GetUserByToken按鈕，將會使用步驟1取得的Token進行身分驗證，並回傳目前登入的User資料。(authenticationType="JwtBearer")
  4. 點擊GetUserByCookie按鈕，將會使用Cookie進行身分驗證，並回傳目前登入的User資料。(authenticationType="Cookies")
  5. 點擊Logout按鈕，將會刪除Cookie，並轉頁至Login頁面。
  
  
## [ASP.NET Core] Authenticate/Authorization 流程範例

本篇範例使用自定義的Middleware/Handler，展示在ASP.NET Core裡進行身分認證/身分授權的步驟流程。

範例原碼：https://github.com/Clark159/CLK.AspNetCoreLab

範例專案：KernelAuthenticationLab

執行結果：
  - ![AuthLab執行結果](https://raw.githubusercontent.com/Clark159/CLK.AspNetCoreLab/master/doc/AuthLab/%E5%9F%B7%E8%A1%8C%E7%B5%90%E6%9E%9C.png)

參考資料：
  
  1. [ASP.NET Core 运行原理解剖:Authentication-雨夜朦朧](https://www.cnblogs.com/RainingNight/p/authentication-in-asp-net-core.html)  
  
