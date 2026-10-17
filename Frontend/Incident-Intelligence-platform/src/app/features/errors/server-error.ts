import { Component } from '@angular/core';
import { ErrorPanelComponent } from '../../shared/components/error-panel';

@Component({
  selector: 'app-server-error',
  imports: [ErrorPanelComponent],
  template: `
    <app-error-panel
      code="500"
      heading="Internal server error"
      description="The control plane encountered an unexpected fault while processing telemetry."
      state="Upstream service fault on the primary node"
      statusTag="STATUS: ERR_FAULT"
      eyeline="CORE // CONTROL PLANE"
    />
  `,
})
export class ServerErrorComponent {}
