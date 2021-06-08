# CLK.AspNetCoreLab

## [ASP.NET Core] Cookie/JwtBearer並存的身分驗證範例

本篇範例展示如何在ASP.NET Core裡，同時使用Cookie及JwtBearer身分驗證機制。

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
  
  
## [ASP.NET Core] Authenticate/Authorization 步驟流程範例

本篇範例使用自定義的Middleware/Handler，展示在ASP.NET Core裡進行身分認證/身分授權的步驟流程。

範例原碼：https://github.com/Clark159/CLK.AspNetCoreLab

範例專案：KernelAuthenticationLab

執行結果：
  - ![AuthLab執行結果](https://raw.githubusercontent.com/Clark159/CLK.AspNetCoreLab/master/doc/AuthLab/%E5%9F%B7%E8%A1%8C%E7%B5%90%E6%9E%9C.png)

參考資料：
  
  1. [ASP.NET Core 运行原理解剖:Authentication-雨夜朦朧](https://www.cnblogs.com/RainingNight/p/authentication-in-asp-net-core.html)  
  
