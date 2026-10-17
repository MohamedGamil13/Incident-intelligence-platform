import { Component } from '@angular/core';
import { ErrorPanelComponent } from '../../shared/components/error-panel';

@Component({
  selector: 'app-forbidden',
  imports: [ErrorPanelComponent],
  template: `
    <app-error-panel
      code="403"
      heading="Access forbidden"
      description="Your operator identity lacks the required role scope to enter this console segment."
      state="Authorization scope insufficient for resource"
      statusTag="STATUS: ERR_DENIED"
      eyeline="IDENTITY // POLICY ENFORCEMENT"
    />
  `,
})
export class ForbiddenComponent {}
