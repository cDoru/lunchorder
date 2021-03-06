import {Directive, ElementRef, Input, HostListener} from '@angular/core';
import {Observable} from 'rxjs/Rx';

@Directive({
    selector: '[stick-rx]'
})
export class StickRxDirective {

    constructor(private _element: ElementRef, private window: Window) {

        this.subscribeForScrollEvent();
    }

    stickyOffset: number; 
    subscribeForScrollEvent() {
        var obs = Observable.fromEvent(window, 'scroll');
        this.stickyOffset = this._element.nativeElement.offsetTop;

        obs.subscribe((e : any) => this.handleScrollEvent(e));
    }

    handleScrollEvent(e: any) {
        var scroll = this.window.pageYOffset;

        if (scroll >= this.stickyOffset) { this._element.nativeElement.classList.add('stick-rx'); }

        else { this._element.nativeElement.classList.remove('stick-rx'); }
    }
}