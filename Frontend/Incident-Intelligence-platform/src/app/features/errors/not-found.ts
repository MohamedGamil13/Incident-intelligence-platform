import { Component } from '@angular/core';
import { ErrorPanelComponent } from '../../shared/components/error-panel';

@Component({
  selector: 'app-not-found',
  imports: [ErrorPanelComponent],
  template: `
    <app-error-panel
      code="404"
      heading="Page not found"
      description="The page you're looking for doesn't exist or has been moved to an isolated segment."
      state="Route unmapped in cluster gateway"
      statusTag="STATUS: ERR_UNRESOLVED"
    />
  `,
})
export class NotFoundComponent {}
