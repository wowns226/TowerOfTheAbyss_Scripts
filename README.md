>외부 에셋으로 인해서 스크립트만 따로 추출한 저장소입니다.<br/>
>Git 커밋 이력은 별도의 csv파일(commit history.csv)에 첨부하였습니다.
# Tower Of The Abyss (심연의 탑)

소개 영상 : [https://youtu.be/4xPeUJPAWPo](https://youtu.be/hKK0JInDPKk)
<br>
인게임 영상 : https://youtu.be/o91kn8_SLjM
<br>
데모 링크 : https://store.steampowered.com/app/2709030/Tower_of_the_abyss/
<br/>
<br/>

## 🖥️ 프로젝트 소개

로그라이크 방식의 게임으로 매 라운드 적을 막아서 돈을 모으고

모은 돈을 바탕으로 유닛 뽑기와, 유물, 연구, 업그레이드 등 전략적으로 소비하여 진행하는 게임입니다.

스토리 모드와 무한 모드의 2가지로 이루어져 있으며

스토리 모드는 이야기를 바탕으로 게임이 진행되고 80라운드가 마지막입니다.

무한 모드는 제한 없이 계속 진행이 가능하고 난이도가 좀 더 높습니다.

<br><img src = "https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/83fb7f55-7089-469b-942d-d715dafb0ac4" width="50%" height="50%"> (게임 메인화면)
<br/>
<br/>

## 🕰️ 개발 기간
* 2022.11 - 2024.02
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
 - AWS EC2
 - Jenkins
 - Steamworks (유저 데이터, 랭킹 등..)
 
<br/>
<br/>

# ⚙ System Architecture

- CloudFront를 사용해서 어드레서블로 빌드된 에셋 파일을 받을때 캐싱해서 부분을 받아올 수 있도록 세팅

<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/8eec46a9-5203-4350-9d03-48a96777797f" width="50%" height="50%">

<br/>
<br/>

# ❗ 핵심 기능

## 스토리

- 게임 내 스토리 전달에 사용할 웹툰 연출
<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/a1240afc-5ff9-4c9e-86ab-1a2ec0bd9b9f" width="60%" height="60%">
<br/>
<br/>

## 리더보드

- 유저 별 랭킹 확인 가능
<br>
<br/>

## 시네머신

- 게임 내 연출, 스토리 설명, 엔딩, 쿠키 영상 등에 시네머신 사용
<br/>
<br/>

## 세팅 값 조절

- 세팅값 변경이 있을 경우 SAVE 활성화 (SAVE 하지 않고 나갈 경우 알림 창 표시 -> 저장 여부 선택 가능)
- 세팅창에서 즉시 적용되는 옵션(밝기 등..)과 SAVE 시 적용되는 옵션(해상도 등..) 분리
- 세팅값을 초기값으로 RESET 시켜주는 기능
- 설정된 키값은 json 형태로 암호화 되어서 파일에 저장이 되고 게임 시작 시 로드되어서 사용

#### 언어, 해상도 설정

- 유니티 로컬라이제이션을 이용해서 언어 변경 구현(영어, 한국어, 일본어, 중국어, 독일어, 러시아어)
- 해상도에 맞춰서 UI의 레이아웃이 자연스럽게 잡히도록 구현

<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/25c3b8b3-6ec7-4e95-9640-79693f404877" width="60%" height="60%">
<br>
<br>

#### 사운드 설정

- 배경음, 효과음 등등 사운드 별로 볼륨 조절 가능
<br>

#### 키 바인딩 변경, 민감도 조절

- 원하는 키로 설정된 키 값을 변경 가능
- 마우스, 키보드 민감도 설정 가능
<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/54c723f0-ec21-4632-af43-f4d5d9d4d6a8" width="60%" height="60%">
<br>
<br>

#### 그래픽 설정

- Low,High,Ultra 등등 옵션에 따라서 fps나 퀄리티 조정
- 스킬 이펙트 on/off (단순히 랜더링만 하지 않는게 아니라 실제 스킬 이펙트 자체를 생성 하지 않음)
- 엑스트라 스킬 연출 on/off
- 마우스 이펙트 on/off
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/916f07d0-88f2-4898-8e02-e855d2775f4a" width="60%" height="60%">

<br/>
<br/>

## 게임 모드 변경

- 스토리, 무한 2가지 모드가 존재
- 모드 변경 시 포스트 프로세싱 효과를 사용해서 분위기 연출, 사운드 변경
- 모드 별로 최대 라운드 수, 효과 등 차이가 존재

<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/037a996d-e854-4a6a-a61b-d0d080e6cc19" width="60%" height="60%">

<br/>
<br/>

## 인게임

#### 인게임 UI 기능

- 좌측 하단 성의 HP 확인 가능 (이벤트 형식으로 hp 변경쪽에 등록)
- 좌측 각 유닛의 HP, 죽었을 경우 부활 시간 확인 가능(이벤트 형식으로 각 유닛에 등록)
- 좌측 각 유닛 등급별로 색상 구분
- 게임 배속, 유닛 사거리 확인 UI, 일시 정지 기능 추가(배속은 전체 시간 관리 매니저에 등록하여 UI에서 타임 스케일을 바꾸더라도 영향을 받지 않도록 구현)
- 우측 하단에 유닛 특수 스킬 등록(이벤트 형식으로 유닛 추가쪽에 등록)
- 유닛 아이콘을 클릭하면 카메라가 해당 유닛으로 포커싱 되도록 기능 구현
- 유닛, 버프 지점을 클릭할 경우 간략한 정보를 표시해주는 UI 구현

<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/535bea9d-1da6-47f5-a513-1e2f8c92bad7" width="60%" height="60%">
<br>
<br>

#### 전투

- 물리를 사용하지 않고 로직으로 전투 처리
- 다익스트라에 가중치를 활용해서 경로 미리 저장 후 사용
- 아군 유닛은 항상 최단거리로 이동, 적 유닛은 최단거리에서 크게 벗어나지 않는 선에서 무작위로 이동하도록 설정
- 적은 공격 -> 이동 / 아군은 이동 -> 공격 순으로 fsm 설계 (특정 상황에서는 조금 변동이 있음)
- 적은 피해를 입을 경우 가깝다면 그 쪽을 우선적으로 가도록 처리
- 스킬 범위와 데미지는 스크립터블 오브젝트를 활용해서 처리
- 스킬은 각각 우선순위와 가중치가 존재해서 같은 우선순위 일 경우 가중치에 의해 무작위로 시전 되도록 구현

<br>
<br>

#### 보스

- 5라운드마다 보스 등장
- 보스를 잡을 경우 특별한 보상(카드, 골드) 획득
- 보스는 본인만의 패턴, 스킬이 존재

<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/a60482b7-ed1f-4765-9511-686fd63a76aa" width="60%" height="60%">
<br>
<br>

#### 중립 보스

- 아군/적 관계없이 공격하는 보스 생성
- 보스를 잡을 경우 특별한 보상(버프, 골드) 획득
- 중립 보스는 특정 위치에 생성되고 먼저 공격하기 전까지는 공격하지 않도록 구현
- 게임은 중립보스를 잡지 않아도 진행 할 수 있도록 구현
- 시네머신을 통해서 등장 연출

<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/73b1f75a-df69-4e17-af64-9ebe46f6a785" width="60%" height="60%">
<br>
<br>

#### 카드

- 카드마다 능력이 존재(공격, 회복, 건물 생성, 버프 등..)
- 카드 사용 시 시간을 정지하고 사용한 카드를 큐에 저장해서 한번에 처리
- 카드 정보를 표시할 UI 제작

<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/f84fd3d5-1c86-4609-9d4c-691df424ef3d" width="60%" height="60%">
<br/>
<br/>

#### 의뢰

- 난이도 별로 의뢰 구현(특정 몹을 소환하고 클리어 시 골드 획득)

<br>
<br/>
<br/>

#### 일지

- 게임 내 스토리를 보여줄 용도
- 여러가지 스토리를 특정 조건에 의해 분기 처리해서 상황에 맞춘 스토리를 보여주게 구성

<br>
<br/>
<br/>

#### 전투 연출

- 보스별로 패턴 구현(가중치와 조건에 의해서 공격 패턴이 결정 됨)
- 직선 스킬의 경우에는 라인 랜더러를 사용해서 빨강색으로 예측선을 표시
- 캐스팅 스킬은 캐스팅 바를 구현해서 표시
- 스킬이 적, 맵에 닿았을 때 이펙트 발생 구현(콜라이더를 사용하지 않아서 Bounds에 맵 데이터를 넣고 사용)
- 상황에 따른 적 애니메이션 구현
- 유닛 이동 시 타임 스케일을 낮추고 이동 가능한 포인트 표시
- HP Bar, Casting Bar, 데미지 UI, 버프는 항상 최상단에 표시되도록 처리
- 유닛 하이라이팅 효과, 적이 앞에 있을 경우 투시되게 표시(쉐이더 에셋 사용)
- 단축키를 사용해서 카메라를 해당 유닛으로 이동시키는 유닛 포커싱 기능 추가
- 줌 블러, 카메라 흔들림, 타임 스케일을 조정해서 타격감 표현
- 타격 시 적 이동속도를 늦추고 잠시 동안 유닛의 오버레이 컬러를 빨강으로 줘서 타격감 표현

<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/73d5ce91-55b6-4ab6-8b5d-f1b508c88170" width="60%" height="60%">
<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/0982c588-5800-40db-a57f-025652fc1f58" width="60%" height="60%">
<br/>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/cd5d0a75-3da7-462b-a15d-6636901d30e3" width="60%" height="60%">
<br/>
<br/>

## 게임 내 시스템

#### 튜토리얼

- 텍스트보다는 직접 전투를 진행하면서 배울 수 있도록 구현
- 진행 상태를 저장해서 최초 한 번만 진행하도록 구현(서버에 저장)
<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/423910a6-e649-44cc-8ce2-36d9c22a04f3" width="60%" height="60%">
<br>
<br>

#### 유닛 특성

- 각각의 유닛별로 고유한 개성(패시브)과 스킬을 보유하도록 구현
- 각각의 스킬은 유닛에 의존하지 않도록 독립적으로 구현해서 다른 유닛의 스킬을 가져와서 사용하는게 가능하도록 구현
<br>

#### 프로필

- 유저의 정보 확인 가능
- 기록 데이터 확인 가능
<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/e13b241b-ec0a-45ec-a0b1-9c64e634b059" width="60%" height="60%">
<br>
<br>

#### 연구

- 특정 연구를 잠금 해제해서 추가적인 효과를 얻는 시스템
- 1개의 연구에는 여러개의 하위 연구가 있어서 원하는 효과를 취사 선택하여 전략적인 선택 가능
<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/148f2042-36b3-446c-a21a-c908a5f3cf58" width="60%" height="60%">
<br>
<br>

#### 유닛

- 유닛의 현재 스텟과 스킬 확인 가능
- 각각의 스텟을 업그레이드 가능(업그레이드 수치는 유닛, 타입별로 다르게 적용되도록 구현)
- 유닛 슬롯을 늘리거나 뽑기로 특정 등급의 유닛을 얻을 수 있도록 구현

<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/8f4e4c3b-a082-4e98-bbd6-15f15ca134cb" width="60%" height="60%">
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/f953abd4-7221-4758-ab3e-4663a53ed9d9" width="60%" height="60%">
<br>
<br>

#### 건물

- 건물의 스텟과 스킬 확인 가능
- 건물의 스텟 업그레이드 가능
- 공격, 방어, 실드 타입 존재
- 각 타입별로 상태머신을 구현해서 최적의 행동을 할 수 있도록 구현
- 드론을 추가해서 아군 건물을 수리하거나 디버프, 버프를 사용할 수 있음(드론은 무적상태이며 적정 행동을 알아서 수행하도록 구현)

<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/8e27eb64-e83b-4678-830d-a22188ab45bf" width="60%" height="60%">
<br>
<br>

#### 유물

- 유물의 활성화 여부, 효과, 세트효과 확인 가능
- 라운드 결과 창에서 랜덤으로 유물이 나오고 원하는 유물 선택이 가능하도록 구현
- 하단에 주기적으로 탐색해서 발견할 수 있는 유물 추가(골드, 유물 중 랜덤)
<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/808ab65e-4f8c-4503-86be-d862891dba47" width="60%" height="60%">
<br/>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/4b762be8-4f40-4478-bc76-e636a5cd865e" width="60%" height="60%">
<br/>
<br/>

## 게임 맵 제작

#### 챕터 1

- 평지 맵으로 밝은 분위기를 컨셉으로 제작
<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/b3abadf5-d598-4b27-ace2-a38a87943907" width="60%" height="60%">
<br/>
<br/>

#### 챕터 2

- 복잡한 지형으로 어둡고 신비로운 분위기를 컨셉으로 제작
<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/b92c68f2-8f73-4051-88fb-1e8dc8be1124" width="60%" height="60%">
<br/>
<br/>

#### 챕터 3

- 평지에 언덕이 추가된 구조로 최후의 전장을 컨셉으로 제작
<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/bb9974f2-6445-43ef-ac8d-b6fd22b60eac" width="60%" height="60%">
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
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/62eed0a4-0e5e-4b95-91fe-7cdf7f927978" width="60%" height="60%">
<br>
<br>

#### MVVM 패턴 구현

- 유니티에서 비슷하게나마 MVVM 패턴을 사용할 수 있도록 구현
- DataContainer(Puslisher), DataActiver(Subscriber), DataImager(Subscriber)... 이 존재
- DataContainer의 특정 Item을 구독하고 있다가 이벤트가 발생하면 본인의 상태를 변경 시켜주도록 구현
- CustomEditor를 사용해서 보다 사용하기 쉽도록 구현

<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/e0d0e115-741a-4e97-bf3e-0d5a31e285e3" width="60%" height="60%">
<br/>
<br/>

#### 스킬 스크립터블 오브젝트 편의 기능 추가

- 스킬의 데미지, 간격, 범위를 좀 더 시각적으로 설정 할 수 있게 인스펙터 창 수정
- 마우스 클릭 시, 검은색으로 변경되고 해당 범위에 공격 데미지가 들어가게 제작

<br>
<img src="https://github.com/wowns226/TowerOfTheAbyss_Scripts/assets/60382915/c0a1f979-3469-4fb3-ba3b-7156bd13bddc" width="60%" height="60%">
<br/>
<br/>

## 최적화

- 라이트 프로브 사용
- 라이트 베이크를 사용해서 정적인 오브젝트에 대한 빛 연산 처리
- 정적인 오브젝트에 static batching 사용
- 텍스처 압축 및 해상도 조정
- 매쉬 컴바인을 통한 드로우콜 조정
- 빌드 후 에디터 로그를 통해서 많은 용량을 차지하는 부분 최적화
- 오클루전 컬링 적용
- 아틀라스 맵 추가
- 그래픽 인스턴싱과 랜더링 큐를 이용해서 이펙트 드로우 콜 최적화
- 터레인 to obj 변환을 통한 드로우 콜 최적화
- 프로파일러를 통한 병목 지점 개선
- 프레임 디버거로 드로우 콜 확인 후 개선
- 메모리 프로파일러로 누수되는 부분 체크
- 너무 심하게 가비지 발생하는 부분 확인 후 메모이제이션으로 처리할 수 있는 부분들은 처리
- 사운드가 겹쳐서 한번에 여러 번 재생되는 부분은 재생되는 사운드의 볼륨을 올려주는 식으로 처리

<br/>
<br/>

## 기타

- 난독화 적용(에셋 사용)

<br>

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
