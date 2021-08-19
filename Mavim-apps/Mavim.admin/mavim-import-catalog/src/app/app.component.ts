import { Component, OnInit } from '@angular/core';
import { BroadcastService, MsalService } from '@azure/msal-angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'mavim-import-catalog';
  isLoggedIn = false;

  constructor(
    private broadcastService: BroadcastService,
    private authService: MsalService
  ) {}

  ngOnInit() {
    this.setAccountLoginState();

    this.broadcastService.subscribe('msal:loginSuccess', () => {
      this.setAccountLoginState();
    });

    this.authService.handleRedirectCallback((authError, response) => {
      if (authError) {
        console.error('Redirect Error: ', authError.errorMessage);
        return;
      }

      console.log('Redirect Success: ', response.accessToken);
    });

    if (!this.isLoggedIn) {
      this.authService.loginRedirect();
    }
  }

  setAccountLoginState() {
    this.isLoggedIn = !!this.authService.getAccount();
  }
}
