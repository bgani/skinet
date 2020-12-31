import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';


@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  @Input() totalCount = 0;
  @Input() pageSize = 0;
  @Input() pageNumber = 0;
  @Output() pageChanged = new EventEmitter<number>();

  constructor() { }

  ngOnInit(): void {
  }

  onPagerChanged(event: any){
    // 1. we create an @Output() EventEmitter for this component
    // 2. we emit the data to function that we have in a parent component
    // 3. e.g. in shop component we have a func called onPageChanged, hence the usage in a template of the shop comp will be:
    // (pageChanged) = "onPageChanged($event)"
    this.pageChanged.emit(event.page);
  }

}
