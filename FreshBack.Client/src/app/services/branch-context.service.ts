import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface CurrentBranch {
  id?: number;
  name?: string;
  nameAr?: string;
  nameEn?: string;
}

@Injectable({
  providedIn: 'root'
})
export class BranchContextService {
  private readonly selectedBranch$ = new BehaviorSubject<CurrentBranch | null>(null);

  /** Current branch selected in the layout header (الفرع الحالي). */
  readonly currentBranch$ = this.selectedBranch$.asObservable();

  setSelectedBranch(branch: CurrentBranch | null): void {
    this.selectedBranch$.next(branch);
  }

  getSelectedBranch(): CurrentBranch | null {
    return this.selectedBranch$.value;
  }

  getSelectedBranchId(): number | null {
    const b = this.selectedBranch$.value;
    return b?.id ?? null;
  }
}
