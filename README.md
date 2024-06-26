The UWO Daily Custodian app is designed to digitalize the completion of daily tasks and operations
for the custodial staff at the University of Wisconsin Oshkosh. The app provides various features
tailored to different user roles, including custodians, leads, and supervisors.
** Current only works on android, but should run on Apple in the future **

How to use:
  1. Sign Up/Login: Users can sign up or log in to their accounts based on their roles (custodian, lead, or supervisor).
  2. View Forms: Depending on the user's role, they can view submitted cleaning reports or monitor cleaning progress.
  3. Submit Forms: Custodians can submit daily cleaning reports, including details such as areas cleaned and tasks completed.
  4. Manage Employees: Leads and supervisors can manage custodial staff and track performance.
  5. Generate Reports: Supervisors can generate reports, analyze cleaning trends, and identify areas for improvement.

Here are the database schemas used to store custodian information, not including the two storage buckets "photos" and "files": 
  create table
  public.custodian_forms (
    id bigint generated by default as identity,
    first_name text not null,
    last_name text not null,
    building text not null,
    date timestamp with time zone not null default now(),
    class_boards boolean not null default false,
    class_garbage boolean null default false,
    class_floors boolean null default false,
    class_dusting boolean null default false,
    class_windows boolean null default false,
    class_walls boolean null default false,
    hall_floors boolean null default false,
    hall_garbage boolean null default false,
    hall_dusting boolean null default false,
    hall_walls boolean null default false,
    bath_sinks boolean null default false,
    bath_toilets boolean null default false,
    bath_dusting boolean null default false,
    bath_mirrors boolean null default false,
    bath_ledges boolean null default false,
    bath_dryers boolean null default false,
    bath_vents boolean null default false,
    bath_floors boolean null default false,
    bath_walls boolean null default false,
    bath_curtains boolean null default false,
    bath_shower boolean null default false,
    bath_supply boolean null default false,
    office_vacuum boolean null default false,
    stair_floors boolean null default false,
    stair_railings boolean null default false,
    stair_walls boolean null default false,
    entr_glass boolean null default false,
    entr_floors boolean null default false,
    entr_rugs boolean null default false,
    entr_dusting boolean null default false,
    custodian_name text null,
    constraint custodian_froms_pkey primary key (id)
  ) tablespace pg_default;

  create table
  public.form_relation (
    id bigint generated by default as identity,
    lead_id bigint not null,
    custodian_id bigint null,
    constraint form_relation_pkey primary key (id),
    constraint public_form_relation_custodian_id_fkey foreign key (custodian_id) references custodian_forms (id),
    constraint public_form_relation_lead_id_fkey foreign key (lead_id) references lead_forms (id)
  ) tablespace pg_default;

  create table
  public.lead_forms (
    id bigint generated by default as identity,
    first_name text not null,
    last_name text not null,
    building text not null,
    date timestamp with time zone not null default now(),
    class_boards integer null default 0,
    class_garbage integer null default 0,
    class_floors integer null default 0,
    class_dusting integer null default 0,
    class_windows integer null default 0,
    class_walls integer null default 0,
    hall_floors integer null default 0,
    hall_garbage integer null default 0,
    hall_dusting integer null default 0,
    hall_walls integer null default 0,
    bath_sinks integer null default 0,
    bath_toilets integer null default 0,
    bath_dusting integer null default 0,
    bath_mirrors integer null default 0,
    bath_ledges integer null default 0,
    bath_dryers integer null default 0,
    bath_vents integer null default 0,
    bath_floors integer null default 0,
    bath_walls integer null default 0,
    bath_curtains integer null default 0,
    bath_shower integer null default 0,
    bath_supply integer null default 0,
    office_vacuum integer null default 0,
    stair_floors integer null default 0,
    stair_railings integer null default 0,
    stair_walls integer null default 0,
    entr_glass integer null default 0,
    entr_floors integer null default 0,
    entr_rugs integer null default 0,
    entr_dusting integer null default 0,
    lead_name text null,
    remarks text null,
    constraint lead_forms_pkey1 primary key (id)
  ) tablespace pg_default;

  create table
  public.user_emails (
    id bigint generated by default as identity,
    email text not null,
    role text null,
    constraint user_emails_pkey primary key (id),
    constraint user_emails_email_key unique (email)
  ) tablespace pg_default;


Resource Attributions:
  cleaning.png <a href="https://www.flaticon.com/free-icons/sweeping" title="sweeping icons">Sweeping icons created by Milkghost Studio - Flaticon</a>
  lead.png <a href="https://www.flaticon.com/free-icons/customer" title="customer icons">Customer icons created by Freepik - Flaticon</a>
  supervisor.png <a href="https://www.flaticon.com/free-icons/ceo" title="ceo icons">Ceo icons created by Ch.designer - Flaticon</a>
  delete_employee.png <a href="https://www.flaticon.com/free-icons/delete" title="delete icons">Delete icons created by Pixel perfect - Flaticon</a>
  edit_employee.png <a href="https://www.flaticon.com/free-icons/edit" title="edit icons">Edit icons created by Pixel perfect - Flaticon</a>