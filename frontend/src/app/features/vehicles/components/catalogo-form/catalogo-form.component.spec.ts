import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogoFormComponent } from './catalogo-form.component';

describe('CatalogoFormComponent', () => {
  let component: CatalogoFormComponent;
  let fixture: ComponentFixture<CatalogoFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CatalogoFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CatalogoFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
