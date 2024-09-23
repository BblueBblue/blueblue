using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector; //��� �߰����ְ�
using System;

namespace OdinInspectorSample
{
    public class Test
    {
        [PreviewField(50, ObjectFieldAlignment.Right)]
        public Texture icon;
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public Sprite image;
        public string name;
        [VerticalGroup("Info")]
        public int age;
        [VerticalGroup("Info")]
        public float weight;
        [VerticalGroup("Info")]
        public float height;
    }
    public class OdinSample : SerializedMonoBehaviour //��� �ٱ��ְ�
    {
        [Title("Sample")]
        [InfoBox("Simple Example")]
        [DetailedInfoBox("Detail Example", "�� �������� ������ ������ �� �� �ֽ��ϴ�.\n�� �̿��غ�����.")]
        public int a = 0;

        [Title("Blank Between Two Value in Inspector")]
        [Space]
        public int b = 0;
        [PropertySpace(10.0f, 10.0f)]
        public int c = 0;

        [Title("Readonly")]
        [ReadOnly]
        public string _a = "���ĺ�����. �ȵɰ�~";

        [Title("Dropdown")]
        [ValueDropdown("DropDownNum")]
        public int d;
        private int[] DropDownNum = new int[] { -1, 0, 1 };

        [Title("Search")]
        [Searchable]
        public List<string> itemList = new List<string>()
        {
            "sword", "arrow", "shield"
        };

        [Title("Enum Utility")]
        public enum Weapon
        {
            sword,
            arrow,
            shield
        };
        [EnumPaging]
        public Weapon w;
        [EnumToggleButtons]
        public Weapon _w;

        [Title("Table")]
        [TableList]
        public List<Test> t = new List<Test>();

        [Title("More about info box")]
        [InfoBox("�����ؾ��� �� ���� ��� ����ǥ", InfoMessageType.Warning)]
        [InfoBox("���� Ʋ���� ���� ���� ����ǥ", InfoMessageType.Error)]
        [InfoBox("���� ������ �����ڽ�", InfoMessageType.None)]
        public bool toggle;
        [InfoBox("����� Ȱ��ȭ�غ�����")]
        [InfoBox("¥����", "toggle")]
        public bool aa; //�����ڽ��� ���ܵ� �� ������
    }
}
