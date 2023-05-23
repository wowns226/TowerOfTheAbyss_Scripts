# Tower Of The Abyss (심연의 탑)
외부 에셋으로 인해서 스크립트만 따로 추출한 저장소입니다.
Git 커밋 이력은 별도의 csv파일(commit history.csv)에 첨부하였습니다.

소개 영상 : https://youtu.be/4xPeUJPAWPo
<br/>

## 🖥️ 프로젝트 소개

로그라이크 류 디펜스 게임으로 매 라운드 적을 막아서 돈을 모으고

모은 돈을 바탕으로 유닛 뽑기와, 유물, 연구, 업그레이드 등 전략적으로 소비하여 진행하는 게임입니다.

스토리 모드와 무한 모드의 2가지로 이루어져 있으며

스토리 모드는 이야기를 바탕으로 게임이 진행되고 100라운드가 마지막입니다.

무한 모드는 제한 없이 계속 진행이 가능합니다.

<br><img src = "https://github.com/wowns226/DefenceCastle/assets/60382915/b96de733-022f-4454-9d07-2ef640032ecc" width="50%" height="50%"> (게임 메인화면)
<br/>
<br/>

## 🕰️ 개발 기간
* 2022.11 - 2023.06(예상 )
<br/>
<br/>

## 🧑‍🤝‍🧑 맴버구성
 - 팀원  : 신용욱 - 개발
 - 팀원  : 임재준 - 개발
<br/>
<br/>

# 🔨 Enviroment + Tech Skills

### 개발 환경

- Unity
- Visual Studio
- Rider

### DevOps

- Git

### ETC

 - DynamoDB
 - AWS S3
 
<br/>
<br/>

# ⚙ System Architecture
<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/fe725ffd-3b54-4377-a596-f6c90432b755" width="50%" height="50%">

<br/>
<br/>

# ❗ 핵심 기능

## 세팅 값 조절

- 세팅값 변경이 있을 경우 SAVE 활성화 (SAVE 하지 않고 나갈 경우 알림 창 표시 -> 저장 여부 선택 가능)
- 세팅값을 초기값으로 RESET 시켜주는 기능
- 설정된 키값은 json 형태로 암호화 되어서 파일에 저장이 되고 게임 시작 시 로드되어서 사용

#### 언어, 해상도 설정

- 유니티 로컬라이제이션을 이용해서 언어 변경 구현
- 해상도에 맞춰서 UI의 레이아웃이 자연스럽게 잡히도록 구현

<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/d8907f87-2493-4828-a675-57ab9abf7f82" width="60%" height="60%">
<br>
<br>

#### 사운드 설정

- 볼륨 조절
<br>

#### 키 바인딩 변경, 민감도 조절

- 원하는 키로 설정된 키 값을 변경 가능
- 마우스, 키보드 민감도 설정 가능
<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/f6a37cb2-891b-46e8-b617-e2960b763412" width="60%" height="60%">
<br>
<br>

#### 그래픽 설정

- 스킬 이펙트 on/off (단순히 랜더링만 하지 않는게 아니라 실제 스킬 이펙트 자체를 인스턴스 하지 않음)
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/cc64bd09-36a7-4223-876c-49163bf5ad93" width="60%" height="60%">

<br/>
<br/>

## 게임 모드 변경

- 스토리, 무한 2가지 모드가 존재
- 모드 변경 시 포스트 프로세싱 효과를 사용해서 분위기 연출, 사운드 변경
- 모드 별로 최대 라운드 수, 효과 등 차이가 존재

<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/540ae2ba-8b70-4d36-8e47-ddc242f093dc" width="60%" height="60%">

<br/>
<br/>

## 인게임

#### 인게임 UI 기능

- 좌측 상단 성의 HP 확인 가능 (이벤트 형식으로 hp 변경쪽에 등록)
- 좌측 각 유닛의 HP, 죽었을 경우 부활 시간 확인 가능(이벤트 형식으로 각 유닛에 등록)
- 좌측 각 유닛 등급별로 색상 구분
- 게임 배속, 유닛 사거리 확인 UI, 일시 정지 기능 추가(배속은 전체 시간 관리 매니저에 등록하여 UI에서 타임 스케일을 바꾸더라도 영향을 받지 않도록 구현)
- 우측 하단에 유닛 별 특수 스킬 등록(이벤트 형식으로 유닛 추가쪽에 등록)

<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/90af0628-174e-483c-9450-0b99d2f71215" width="60%" height="60%">
<br>
<br>

#### 전투

- 물리를 사용하지 않고 로직으로 전투 처리
- 다익스트라에 가중치를 활용해서 경로 미리 저장 후 사용
- 적은 공격 -> 이동 / 아군은 이동 -> 공격 순으로 fsm 설계 (특정 상황에서는 조금 변동이 있음)
- 적은 피해를 입을 경우 가깝다면 그 쪽을 우선적으로 가도록 처리
- 스킬 범위와 데미지는 스크립터블 오브젝트를 활용해서 처리

<br>
<br>

#### 전투 연출

- 직선 스킬의 경우에는 라인 랜더러를 사용해서 빨강색으로 예측선을 표시
- 캐스팅 스킬은 캐스팅 바를 구현해서 표시
- 스킬이 적, 맵에 닿았을 때 이펙트 발생 구현(콜라이더를 사용하지 않아서 Bounds에 맵 데이터를 넣고 사용)
- 상황에 따른 적 애니메이션 구현
- 유닛 이동 시 타임 스케일을 낮추고 이동 가능한 포인트 표시
- HP Bar, Casting Bar, 데미지 UI 는 항상 최상단에 표시되도록 처리
- 유닛 하이라이팅 효과, 적이 앞에 있을 경우 투시되게 표시(쉐이더 에셋 사용)

<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/23ec7fcc-1a0a-47a9-a51a-ff2ff1483a3c" width="60%" height="60%">
<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/2036d036-b1d9-43e7-ad69-c0677153986e" width="60%" height="60%">
<br/>
<br/>

## 게임 내 시스템

#### 유닛 특성

- 각각의 유닛별로 고유한 개성(패시브)과 효과를 보유하도록 구현
<br>

#### 프로필

- 유저의 정보 확인 가능
- 추후 업적이 추가되면 확인 가능(현재는 아직 스팀등록이 안되어서 관련 API 사용 불가로 인한 미구현 상태)
<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/cd1a0ff6-788d-43a2-a5a5-ae099fc0ca27" width="60%" height="60%">
<br>
<br>

#### 연구

- 특정 연구를 잠금 해제해서 추가적인 효과를 얻는 시스템
- 1개의 연구에는 여러개의 하위 연구가 있어서 원하는 효과를 취사 선택하여 전략적인 선택 가능
<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/936ae5b8-e3a9-487b-bba1-4557721b47dd" width="60%" height="60%">
<br>
<br>

#### 유닛

- 유닛의 현재 스텟과 스킬 확인 가능
- 각각의 스텟을 업그레이드 가능(업그레이드 수치는 유닛, 타입별로 다르게 적용되도록 구현)
- 유닛 슬롯을 늘리거나 뽑기로 특정 등급의 유닛을 얻을 수 있도록 구현

<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/c9276afc-27b3-4c7e-9110-8c12edba237b" width="60%" height="60%">
<br>
<br>

#### 건물

- 건물의 스텟과 스킬 확인 가능
- 건물의 스텟 업그레이드 가능(추후, 방벽과 공격타워 등 추가 예정)
- 드론을 추가해서 아군 건물을 수리하거나 디버프, 버프를 사용할 수 있음(드론은 무적상태이며 적정 행동을 알아서 수행하도록 구현)
<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/13cf54e5-0b24-47de-a4cf-c81507511dd3" width="60%" height="60%">
<br>
<br>

#### 유물

- 유물의 활성화 여부, 효과, 세트효과 확인 가능
- 유물마다 판매가 가능하도록 구현(등급마다 금액 상이)
- 라운드 결과 창에서 랜덤으로 유물이 나오고 원하는 유물 선택이 가능하도록 구현
<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/22b96aa3-7c7e-43e0-ab7e-f241534dbac7" width="60%" height="60%">
<br/>
<br/>

## 사용 기술

#### 세팅 매니저 구현

- 세팅 매니저에서 ISetting 인터페이스를 구현한 클래스의 값들을 관리
- 원하는 필드나 프로퍼티들만 세팅되도록 설정이 가능하고, Image나 GameObject도 설정값으로 넣을 수 있도록 구현
- MonoBehaviour를 상속받지 않는 클래스에도 사용이 가능
- CustomEditor를 이용해서 인스펙터에서 보다 쉽게 데이터 관리가 가능하도록 구현
- Open 버튼을 활용해서 해당 프리팹을 바로 열어주는 기능 구현(프리팹으로 존재하는 클래스만 가능)

<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/20c94f41-8dca-4689-9761-70b21c697ca4" width="60%" height="60%">
<br>
<br>

#### MVVM 패턴 구현

- 유니티에서 비슷하게나마 MVVM 패턴을 사용할 수 있도록 구현
- DataContainer(Puslisher), DataActiver(Subscriber), DataImager(Subscriber)... 이 존재
- DataContainer의 특정 Item을 구독하고 있다가 이벤트가 발생하면 본인의 상태를 변경 시켜주도록 구현
- CustomEditor를 사용해서 보다 사용하기 쉽도록 구현

<br>
<img src="https://github.com/wowns226/DefenceCastle/assets/60382915/bc78eb98-c12b-41fc-90a1-242d67272ecb" width="60%" height="60%">
<br/>
<br/>

## 최적화

- 라이트 프로프 사용
- 라이트 베이크를 사용해서 정적인 오브젝트에 대한 빛 연산 처리
- 정적인 오브젝트에 static batching 사용
- 텍스처 압축 및 해상도 조정
- 매쉬 컴바인을 통한 드로우콜 조정
- 빌드 후 로그를 통해서 많은 용량을 차지하는 부분 최적화
- 오클루전 컬링 적용
- 아틀라스 맵 추가 예정(아이템 추가가 끝나면 적용)

<br/>
<br/>

# License

Copyright (C) <2023>  
Authors : Shin YongUk <dyddyddnr@naver.com> <br>
          Lim jaejun <wowns226@naver.com>
 
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
 
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.
 
You should have received a copy of the GNU General Public License
along with this program. If not, see <http://www.gnu.org/licenses/>
