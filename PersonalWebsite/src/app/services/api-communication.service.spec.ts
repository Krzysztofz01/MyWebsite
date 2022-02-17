import { TestBed } from '@angular/core/testing';

import { ApiCommunicationService } from './api-communication.service';

describe('ApiCommunicationService', () => {
  let service: ApiCommunicationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApiCommunicationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
