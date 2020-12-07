import { AfterViewInit, Component, ElementRef, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';

@Component({
  selector: 'animated-digit',
  templateUrl: './animated-digit.component.html',
  styleUrls: ['./animated-digit.component.css']
})
export class AnimatedDigitComponent implements AfterViewInit, OnChanges {
  @Input() duration: number;
  @Input() digit: number;
  @Input() steps: number;
  @ViewChild("animatedDigit", { static: true }) animatedDigit: ElementRef;

  private animateCount(): void {
    if(!this.duration) {
      this.duration = 1000;
    }

    if(typeof this.digit === "number") {
      this.counterFunc(this.digit, this.duration, this.animatedDigit);
    }
  }

  private counterFunc(endValue: number, durationMs: number, element: ElementRef): void {
    if(!this.steps) {
      this.steps = 12;
    }

    const stepCount = Math.abs(durationMs / this.steps);
    const valueIncrement = (endValue - 0) / stepCount;
    const sinValueIncrement = Math.PI / stepCount;

    let currentValue = 0;
    let currentSinValue = 0;

    function step() {
      currentSinValue += sinValueIncrement;
      currentValue += valueIncrement * Math.sin(currentSinValue) ** 2 * 2;

      element.nativeElement.textContent = Math.abs(Math.floor(currentValue));

      if(currentSinValue < Math.PI) {
        window.requestAnimationFrame(step);
      }
    }

    step();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(changes["digit"]) {
      this.animateCount();
    }
  }
  
  ngAfterViewInit(): void {
    if(this.digit) {
      this.animateCount();
    }
  }

}
