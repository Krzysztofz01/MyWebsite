import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SocialmediaSectionComponent } from './socialmedia-section.component';

describe('SocialmediaSectionComponent', () => {
  let component: SocialmediaSectionComponent;
  let fixture: ComponentFixture<SocialmediaSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SocialmediaSectionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SocialmediaSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
