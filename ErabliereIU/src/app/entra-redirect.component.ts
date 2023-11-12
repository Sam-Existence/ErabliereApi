// This component is part of @azure/msal-angular and can be imported and bootstrapped
import { Component, OnInit } from "@angular/core";
import { MsalService } from "@azure/msal-angular";

@Component({
  selector: 'entra-redirect', // Selector to be added to index.html
  template: ''
})
export class EntraRedirectComponent implements OnInit {
  
  constructor(private authService: MsalService) { }
  
  ngOnInit(): void {    
      this.authService.handleRedirectObservable().subscribe();
  }
  
}