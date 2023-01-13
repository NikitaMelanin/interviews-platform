import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {ICandidate, IVacancy} from "../../../_types";
import {FormBuilder, FormControl, FormGroup} from "@angular/forms";
import {mainRoutes} from "../routes";


@Component({
  selector: 'app-nav-menu',
  templateUrl: './candidates.component.html',
  styleUrls: ['./candidates.component.css']
})
export class CandidatesComponent implements OnInit {
  candidates!: ICandidate[];
  isLoaded = false;
  findForm!: FormGroup;
  filter: string = '';
  id!: string;
  sideRoutes = mainRoutes.filter(x => x.side);
  routes = mainRoutes.filter(x => !x.side);

  constructor(private readonly fb: FormBuilder,
              private readonly httpClient: HttpClient,
              private readonly route: ActivatedRoute,
              private readonly router: Router) {
  }


  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (this.id === null) {
      this.router.navigate(['']);
      return;
    }
    this.id = id as string;
    this.httpClient.get<ICandidate[]>('https://localhost:44423/api/interviewees').subscribe((x) => {
      this.candidates = x;
      this.isLoaded = true;
    });
  }
  deleteCandidate() {
    this.httpClient
      .delete("https://localhost:44423/api/interviewees/" + this.id, {})
      .subscribe(() => {
        this.router.navigate(['dashboard/candidates']);
      });
  }

  onSubmit() {
  }


}
