import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss'],
})
export class ServerErrorComponent implements OnInit {
  error: any;
  // navigationExtras only available in the constructor, in ngOnInit it will be undefined
  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    // be defensive
    this.error =
      navigation &&
      navigation.extras &&
      navigation.extras.state &&
      navigation.extras.state.error;
  }

  ngOnInit(): void {}
}
