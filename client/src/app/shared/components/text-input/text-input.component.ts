import { Component, ElementRef, Input, OnInit, Self, ViewChild, ɵɵtrustConstantResourceUrl } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements OnInit, ControlValueAccessor {
  @ViewChild('input', {static: true}) input: ElementRef = null as any;
  @Input() type = 'text';
  @Input() label: string = null as any;
  
  // by injecting control we get an access to control itself, that means we'll be able to access its properties & validate it insid this component
  // @Self decorator is for angular dependency injection, and ng is gonna look for where to locate what it's gonna inject into itself
  // if we already have a service activated somewhere in app, it is gonna walk up the tree of the DI hierarchy looking for something that matches what we're injecting here
  // but if we use @Self decorator here, it's only gonna use it inside itself and not look for any other shared dependencies already in use
  // that guarantees that we're working with the very specefic control that we're injecting in here
  constructor(@Self() public controlDir: NgControl) {

    // binds this to our class and now we've got accesss to our control directives inside our component & template
    this.controlDir.valueAccessor = this;
   }

  ngOnInit(): void {
    const control = this.controlDir.control;
    const validators = control?.validator ? [control.validator] : []; 

    // async validator is ones that go to api and check, and it's applied after sync validation
    const asyncValidators = control?.asyncValidator ? [control.asyncValidator] : [];

    control?.setValidators(validators);
    control?.setAsyncValidators(asyncValidators);
    control?.updateValueAndValidity();
  }
 
  onChange(event: any) {
     
  }

  onTouched(){

  }

  writeValue(obj: any): void {
    this.input.nativeElement.value = obj || '';
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }


}
