import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-visits',
  templateUrl: './visits.component.html',
  styleUrls: ['./visits.component.css']
})
export class VisitsComponent implements OnInit {
  members: Partial<Member[]>;
  predicate = 'visited';
  dateParam: string = 'all';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadVisits();
  }

  loadVisits(){
    this.memberService.periodParam = this.dateParam;
    this.memberService.getVisits(this.predicate, this.pageNumber, this.pageSize).subscribe(response =>{
      this.members = response.result;
      this.pagination = response.pagination;
    })
  }

  pagedChanged(event: any) {
    this.pageNumber = event.page;
    this.loadVisits();
  }

}
